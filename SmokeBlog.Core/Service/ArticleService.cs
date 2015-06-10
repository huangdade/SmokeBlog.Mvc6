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

                    if (!string.IsNullOrEmpty(model.Category))
                    {
                        var paras = model.Category.Split(',')
                            .Select(t => new
                            {
                                ArticleID = id,
                                CategoryID = Convert.ToInt32(t)
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

                if (!string.IsNullOrEmpty(model.Category))
                {
                    var paras = model.Category.Split(',')
                        .Select(t => new
                        {
                            ArticleID = model.ID.Value,
                            CategoryID = Convert.ToInt32(t)
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

        public List<ArticleModel> Query(int pageIndex, int pageSize, out int total, ArticleStatus? status, string keywords)
        {
            StringBuilder sb = new StringBuilder("WHERE 1=1");
            DynamicParameters para = new DynamicParameters();

            if (status.HasValue)
            {
                sb.Append(" AND Status=@Status");
                para.Add("@Status", (byte)status.Value);
            }
            if (keywords != null)
            {
                sb.Append(" AND Title LIKE @Keywords");
                para.Add("@Keywords", "%" + keywords + "%");
            }

            var list = this.QueryByCondition(pageIndex, pageSize, out total, sb.ToString(), null, para);

            return list;
        }

        public ArticleModel Get(int id)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT Article.ID, Article.Title, Article.Content, Article.Summary, Article.[From], Article.PostDate, Article.Status, Article.AllowComment, [User].ID, [User].UserName, [User].Nickname
FROM Article WITH(NOLOCK) 
JOIN [User] WITH(NOLOCK) ON Article.UserID=[User].ID
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
                    var article = reader.Read<ArticleModel, UserModel, ArticleModel>((a, u) =>
                    {
                        a.User = u;
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

        private List<ArticleModel> QueryByCondition(int pageIndex, int pageSize, out int total, string where, string orderBy, object parameter)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT @Total = COUNT(1) FROM [Article] WITH(NOLOCK)
#strWhere#;

DECLARE @tb TABLE
(
    ID INT PRIMARY KEY
);

;WITH ids AS
(
    SELECT ID, Row_Number() OVER(#strOrder#) AS RowID FROM [Article] Article WITH(NOLOCK) #strWhere#
)
INSERT INTO @tb
SELECT ID FROM ids WHERE RowID > @Start AND RowID <= @End;

SELECT Article.ID, Article.Title, Article.Content, Article.Summary, Article.[From], Article.PostDate, Article.Status, Article.AllowComment, [User].ID, [User].UserName, [User].Nickname
FROM @tb A
JOIN Article WITH(NOLOCK) ON A.ID = Article.ID
JOIN [User] WITH(NOLOCK) ON Article.UserID=[User].ID
#strOrder#;

SELECT CategoryID, ArticleID FROM CategoryArticle WITH(NOLOCK)
WHERE ArticleID IN (SELECT ID FROM @tb);
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
                    var articleList = reader.Read<ArticleModel, UserModel, ArticleModel>((article, user) =>
                    {
                        article.User = user;
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
