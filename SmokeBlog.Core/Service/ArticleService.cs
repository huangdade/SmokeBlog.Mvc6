using Microsoft.Framework.ConfigurationModel;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Transactions;
using System.Text;
using SmokeBlog.Core.Models.User;
using SmokeBlog.Core.Enums;
using SmokeBlog.Core.Models.CategoryArticle;
using SmokeBlog.Core.Extensions;

namespace SmokeBlog.Core.Service
{
    public class ArticleService : ServiceBase
    {
        private CategoryService CategoryService { get; set; }

        public ArticleService(IConfiguration configuration, CategoryService categoryService)
            : base(configuration)
        {
            this.CategoryService = categoryService;
        }

        public OperationResult<int?> Add(AddArticleRequest model)
        {
            using (var scope = new TransactionScope())
            {
                using (var conn = this.OpenConnection())
                {
                    string sql = @"
INSERT INTO [Article] ( Title,Content,Summary,UserID,[From],PostDate,Status,AllowComment )
VALUES (@Title, @Content, @Summary, @UserID, @From, @PostDate, ISNULL(@Status,1), @AllowComment);

SELECT @@IDENTITY;
";

                    var id = conn.ExecuteScalar<int>(sql, new
                    {
                        model.Title,
                        model.Content,
                        model.Summary,
                        model.UserID,
                        model.From,
                        PostDate = model.PostDate ?? DateTime.Now,
                        Status = model.Status,
                        model.AllowComment
                    });

                    if (model.CategoryIDList.Length > 0)
                    {
                        var paras = model.CategoryIDList
                            .Select(t => new
                            {
                                ArticleID = id,
                                CategoryID = t
                            }).ToArray();

                        string insertCategorySql = @"
INSERT INTO [CategoryArticle] ( ArticleID, CategoryID )
VALUES ( @ArticleID, @CategoryID );
";

                        conn.Execute(insertCategorySql, paras);
                    }

                    scope.Complete();

                    this.CategoryService.ClearCache();

                    return OperationResult<int?>.SuccessResult(id);
                }
            }
        }

        public OperationResult Edit(EditArticleRequest model)
        {
            using (var scope = new TransactionScope())
            using (var conn = this.OpenConnection())
            {
                string sql = @"
UPDATE TOP(1) Article
SET Title=@Title,Content=@Content,Summary=@Summary,UserID=@UserID,[From]=@From,PostDate=ISNULL(@PostDate,PostDate),Status=isnull(@Status,Status),AllowComment=@AllowComment
WHERE ID=@ID;
";
                var rows = conn.Execute(sql, new
                {
                    ID = model.ID.Value,
                    model.Title,
                    model.Content,
                    model.Summary,
                    model.UserID,
                    model.From,
                    PostDate = model.PostDate ?? DateTime.Now,
                    Status = model.Status,
                    model.AllowComment
                });

                if (rows == 0)
                {
                    return OperationResult.ErrorResult("不存在的文章");
                }

                sql = @"
DELETE FROM CategoryArticle WHERE ArticleID=@ID;
";

                conn.Execute(sql, new
                {
                    ID = model.ID.Value
                });

                if (model.CategoryIDList.Length > 0)
                {
                    var paras = model.CategoryIDList
                        .Select(t => new
                        {
                            ArticleID = model.ID.Value,
                            CategoryID = t
                        }).ToArray();

                    string insertCategorySql = @"
INSERT INTO [CategoryArticle] ( ArticleID, CategoryID )
VALUES ( @ArticleID, @CategoryID );
";

                    conn.Execute(insertCategorySql, paras);
                }

                scope.Complete();

                this.CategoryService.ClearCache();

                return OperationResult.SuccessResult();
            }
        }

        public List<ArticleModel> Query(QueryArticleRequest model, out int total)
        {
            StringBuilder sb = new StringBuilder("WHERE Status!=0");
            DynamicParameters para = new DynamicParameters();

            if (model.Status.HasValue)
            {
                sb.Append(" AND Status=@Status");
                para.Add("@Status", (byte)model.Status.Value);
            }
            if (model.Keywords != null)
            {
                sb.Append(" AND Title LIKE @Keywords");
                para.Add("@Keywords", "%" + model.Keywords + "%");
            }

            var list = this.QueryByCondition(model.PageIndex, model.PageSize, out total, sb.ToString(), null, para);

            return list;
        }

        public List<ArticleModel> QueryByCategory(int categoryID, int pageIndex, int pageSize, out int total)
        {
            string where = "WHERE Status=2 AND ID IN (SELECT ArticleID FROM [CategoryArticle] WHERE CategoryID=@CategoryID)";

            var para = new
            {
                CategoryID = categoryID
            };

            var list = this.QueryByCondition(pageIndex, pageSize, out total, where, "ORDER BY PostDate DESC", para);
            
            return list;
        }

        public List<ArticleModel> QueryByAuthor(int userID, int pageIndex, int pageSize, out int total)
        {
            string where = "WHERE Status=2 AND UserID=@UserID";

            var para = new
            {
                UserID = userID
            };

            var list = this.QueryByCondition(pageIndex, pageSize, out total, where, "ORDER BY PostDate DESC", para);

            return list;
        }

        public ArticleModel Get(int id)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
DECLARE @tbComments TABLE
(
    ID INT PRIMARY KEY,
    Total INT,
    Pass INT,
    Junk INT
);

INSERT INTO @tbComments
SELECT ArticleID AS ID, COUNT(1) AS Total, SUM(CASE Status WHEN 1 THEN 1 ELSE 0 END) AS Pass, SUM(CASE Status WHEN 2 THEN 1 ELSE 0 END) AS Junk
FROM [Comment] WITH(NOLOCK) 
WHERE ArticleID = @ID
GROUP BY ArticleID

SELECT Article.ID, Article.Title, Article.Content, Article.Summary, Article.[From], Article.PostDate, Article.Status, Article.AllowComment, [User].ID, [User].UserName, [User].Nickname, A.ID, A.Total, A.Pass, A.Junk
FROM Article WITH(NOLOCK) 
JOIN [User] WITH(NOLOCK) ON Article.UserID=[User].ID
LEFT JOIN @tbComments A ON Article.ID = A.ID
WHERE Article.ID = @ID;

SELECT CategoryID, ArticleID FROM CategoryArticle WITH(NOLOCK)
WHERE ArticleID = @ID;
";

                var para = new
                {
                    ID = id
                };

                using (var reader = conn.QueryMultiple(sql, para))
                {
                    var article = reader.Read<ArticleModel, UserModel, ArticleComments, ArticleModel>((a, u, ac) =>
                    {
                        a.User = u;
                        a.Comments = ac;
                        return a;
                    }).FirstOrDefault();

                    if (article == null)
                    {
                        return null;
                    }

                    var categoryArticleList = reader.Read<CategoryArticleData>().ToList();
                    var categoryDataList = this.CategoryService.GetCategoryList();

                    var query = from ca in categoryArticleList
                                join c in categoryDataList on ca.CategoryID equals c.ID
                                where ca.ArticleID == article.ID
                                select new Models.Category.CategoryModel
                                {
                                    ID = c.ID,
                                    Name = c.Name
                                };
                    article.CategoryList = query.ToList();

                    return article;
                }
            }
        }

        public OperationResult ChangeStatus(ChangeStatusRequest model)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
UPDATE [Article] SET [Status]=@Status
WHERE ID IN @IDs;
";
                var para = new
                {
                    Status = model.Status,
                    IDs = model.ArticleIDList
                };

                var rows = conn.Execute(sql, para);

                if (rows == 0)
                {
                    return OperationResult.ErrorResult("不存在的文章");
                }

                this.CategoryService.ClearCache();
                return OperationResult.SuccessResult();
            }
        }

        public bool IsArticleExisted(int id)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"SELECT 1 FROM [Article] WHERE ID=@ID";
                var para = new
                {
                    ID = id
                };

                return conn.Exist(sql, para);
            }
        }

        private List<ArticleModel> QueryByCondition(int pageIndex, int pageSize, out int total, string where, string orderBy, object parameter)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT @Total = COUNT(1) FROM [Article] WITH(NOLOCK)
#strWhere#;

DECLARE @tbArticle TABLE
(
    ID INT PRIMARY KEY
);

;WITH ids AS
(
    SELECT ID, Row_Number() OVER(#strOrder#) AS RowID FROM [Article] Article WITH(NOLOCK) #strWhere#
)
INSERT INTO @tbArticle
SELECT ID FROM ids WHERE RowID > @Start AND RowID <= @End;

DECLARE @tbComments TABLE
(
    ID INT PRIMARY KEY,
    Total INT,
    Pass INT,
    Junk INT
);

INSERT INTO @tbComments
SELECT ArticleID AS ID, COUNT(1) AS Total, SUM(CASE Status WHEN 1 THEN 1 ELSE 0 END) AS Pass, SUM(CASE Status WHEN 2 THEN 1 ELSE 0 END) AS Junk
FROM [Comment] WITH(NOLOCK) 
WHERE ArticleID IN (SELECT ID FROM @tbArticle)
GROUP BY ArticleID

SELECT Article.ID, Article.Title, Article.Content, Article.Summary, Article.[From], Article.PostDate, Article.Status, Article.AllowComment, [User].ID, [User].UserName, [User].Nickname, D.ID, D.Total, D.Pass, D.Junk
FROM @tbArticle A
JOIN Article WITH(NOLOCK) ON A.ID = Article.ID
JOIN [User] WITH(NOLOCK) ON Article.UserID=[User].ID
LEFT JOIN @tbComments D ON A.ID = D.ID
#strOrder#;

SELECT CategoryID, ArticleID FROM CategoryArticle WITH(NOLOCK)
WHERE ArticleID IN (SELECT ID FROM @tbArticle);
";

                if (string.IsNullOrEmpty(where))
                {
                    where = "";
                }
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "ORDER BY Article.ID DESC";
                }
                sql = sql.Replace("#strWhere#", where).Replace("#strOrder#", orderBy);

                var para = new DynamicParameters(parameter);
                para.Add("@Total", null, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);
                para.Add("@Start", (pageIndex - 1) * pageSize);
                para.Add("@End", pageIndex * pageSize);

                using (var reader = conn.QueryMultiple(sql, para))
                {
                    var articleList = reader.Read<ArticleModel, UserModel, ArticleComments, ArticleModel>((article, user, comments) =>
                    {
                        article.User = user;
                        article.Comments = comments;
                        return article;
                    }).ToList();

                    var categoryArticleList = reader.Read<CategoryArticleData>().ToList();

                    var categoryDataList = this.CategoryService.GetCategoryList();

                    articleList.ForEach(article =>
                    {
                        var query = from ca in categoryArticleList
                                    join c in categoryDataList on ca.CategoryID equals c.ID
                                    where ca.ArticleID == article.ID
                                    select new Models.Category.CategoryModel
                                    {
                                        ID = c.ID,
                                        Name = c.Name
                                    };
                        article.CategoryList = query.ToList();
                    });

                    total = para.Get<int>("@Total");

                    return articleList.ToList();
                }
            }
        }
    }
}
