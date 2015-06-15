using Microsoft.Framework.ConfigurationModel;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SmokeBlog.Core.Extensions;
using System.Text;
using SmokeBlog.Core.Cache;

namespace SmokeBlog.Core.Service
{
    public class CommentService : ServiceBase
    {
        private ArticleService ArticleService { get; set; }

        private ICache Cache { get; set; }

        public CommentService(IConfiguration configuration, ArticleService articleService, Cache.ICache cache)
            : base(configuration)
        {
            this.ArticleService = articleService;
            this.Cache = cache;
        }

        public OperationResult<int?> Add(AddCommentRequest model)
        {
            if (!this.ArticleService.IsArticleExisted(model.ArticleID))
            {
                return OperationResult<int?>.ErrorResult("不存在的文章");
            }
            if (model.ReplyTo.HasValue && !this.IsCommentExisted(model.ReplyTo.Value))
            {
                return OperationResult<int?>.ErrorResult("不存在的评论");
            }

            using (var conn = this.OpenConnection())
            {
                string sql = @"
INSERT INTO [Comment] (ArticleID,ReplyTo,Content,Email,Nickname,PostDate,PostIP,Status,NotifyOnReply)
VALUES (@ArticleID,@ReplyTo,@Content,@Email,@Nickname,@PostDate,@PostIP,@Status,@NotifyOnReply);

SELECT @@IDENTITY;
";

                var para = new
                {
                    model.ArticleID,
                    model.ReplyTo,
                    model.Content,
                    model.Email,
                    model.Nickname,
                    PostDate = DateTime.Now,
                    model.PostIP,
                    Status = 1,
                    model.NotifyOnReply
                };

                var id = conn.ExecuteScalar<int>(sql, para);

                string cacheKey = Helpers.CacheKeyHelper.GetArticleCommentsCacheKey(model.ArticleID);
                this.Cache.Remove(cacheKey);

                return OperationResult<int?>.SuccessResult(id);
            }
        }

        public List<CommentData> Query(QueryCommentRequest model, out int total)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT @Total=COUNT(1) 
FROM [Comment] Comment WITH(NOLOCK)
JOIN [Article] Article WITH(NOLOCK) ON Comment.ArticleID=Article.ID
#strWhere#;

;WITH ids AS
(
    SELECT Comment.ID,Row_Number() OVER(ORDER BY Comment.ID DESC) AS RowID
    FROM [Comment] Comment WITH(NOLOCK)
    JOIN Article Article WITH(NOLOCK) ON Comment.ArticleID=Article.ID
    #strWhere#
)
SELECT A.ID, A.ReplyTo, A.Content,A.Email,A.Nickname,A.PostDate,A.PostIP,A.Status,A.NotifyOnReply,B.ID,B.Title
FROM [Comment] A WITH(NOLOCK)
JOIN Article B WITH(NOLOCK) ON A.ArticleID=B.ID
JOIN ids C ON A.ID=C.ID
WHERE C.RowID > @Start AND C.RowID <= @End
ORDER BY A.ID DESC
";
                StringBuilder sb = new StringBuilder(" WHERE 1=1");
                if (model.Status.HasValue)
                {
                    sb.Append(" AND Comment.Status=@Status");
                }
                if (!string.IsNullOrWhiteSpace(model.Keywords))
                {
                    model.Keywords = "%" + model.Keywords + "%";
                    sb.Append(" AND (Comment.Content LIKE @Keywords OR Comment.Email LIKE @Keywords OR Comment.Nickname LIKE @Keywords OR Article.Title LIKE @Keywords)");
                }

                sql = sql.Replace("#strWhere#", sb.ToString());

                var para = new DynamicParameters(new
                {
                    Start = (model.PageIndex - 1) * model.PageSize,
                    End = model.PageSize * model.PageSize,
                    Status = model.Status,
                    Keywords = model.Keywords
                });

                para.Add("@Total", 0, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput, 4);

                var query = conn.Query<CommentData, CommentSource, CommentData>(sql, (cd, cs) => 
                {
                    cd.Source = cs;
                    return cd;
                }, para);

                total = para.Get<int>("@Total");

                return query.ToList();
            }

        }

        public List<CommentData> GetCommentList(int articleID)
        {
            string cacheKey = Helpers.CacheKeyHelper.GetArticleCommentsCacheKey(articleID);

            var list = this.Cache.Get<List<CommentData>>(cacheKey, ()=>
            {
                using (var conn = this.OpenConnection())
                {
                    string sql = @"
SELECT ID,Content,ReplyTo,Email,Nickname,PostDate,PostIP,Status,NotifyOnReply
FROM [Comment] WITH(NOLOCK)
WHERE ArticleID=@ArticleID
";
                    var para = new
                    {
                        ArticleID = articleID
                    };

                    return conn.Query<CommentData>(sql, para).ToList();                    
                }
            });

            return list;
        }

        public OperationResult ChangeStatus(ChangeStatusRequest model)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
UPDATE [Comment] SET [Status]=@Status
WHERE ID IN @IDs;

SELECT DISTINCT ArticleID FROM [Comment] WITH(NOLOCK)
WHERE ID IN @IDs;
";
                var para = new
                {
                    Status = model.Status,
                    IDs = model.CommentIDList
                };

                var articleIDList = conn.Query<int>(sql, para).ToList();

                if (articleIDList.Count == 0)
                {
                    return OperationResult.ErrorResult("不存在的评论");
                }

                foreach (var articleID in articleIDList)
                {
                    string cacheKey = Helpers.CacheKeyHelper.GetArticleCommentsCacheKey(articleID);
                    this.Cache.Remove(cacheKey);
                }
                
                return OperationResult.SuccessResult();
            }
        }

        public OperationResult DeleteJunk()
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT DISTINCT ArticleID
FROM [Comment] WITH(NOLOCK)
WHERE Status = 2;

DELETE FROM [Comment]
WHERE Status = 2;
";

                var articleIDList = conn.Query<int>(sql).ToList();

                foreach (var articleID in articleIDList)
                {
                    string cacheKey = Helpers.CacheKeyHelper.GetArticleCommentsCacheKey(articleID);
                    this.Cache.Remove(cacheKey);
                }

                return OperationResult.SuccessResult();
            }
        }

        public OperationResult Delete(DeleteCommentRequest model)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT DISTINCT ArticleID 
FROM [Comment] WITH(NOLOCK)
WHERE ID IN @IDs;

UPDATE [Comment]
SET ReplyTo=NULL
WHERE ReplyTo IN @IDs;

DELETE FROM [Comment]
WHERE ID IN @IDs;
";
                var para = new
                {
                    IDs = model.CommentIDList
                };

                var articleIDList = conn.Query<int>(sql, para).ToList();

                if (articleIDList.Count == 0)
                {
                    return OperationResult.ErrorResult("不存在的评论");
                }

                foreach (var articleID in articleIDList)
                {
                    string cacheKey = Helpers.CacheKeyHelper.GetArticleCommentsCacheKey(articleID);
                    this.Cache.Remove(cacheKey);
                }

                return OperationResult.SuccessResult();
            }
        }

        public List<CommentData> GetLatestCommentList(int count)
        {
            string cacheKey = Helpers.CacheKeyHelper.GetLatestCommentsCacheKey();
            var list = this.Cache.Get<List<CommentData>>(cacheKey);

            if (list == null)
            {
                using (var conn = this.OpenConnection())
                {
                    string sql = @"
SELECT TOP 100 A.ID,A.Content,A.ReplyTo,A.Email,A.Nickname,A.PostDate,A.PostIP,A.Status,A.NotifyOnReply,B.ID,B.Title
FROM [Comment] WITH(NOLOCK)
ORDER BY ID DESC
";
                    list = conn.Query<CommentData, CommentSource, CommentData>(sql, (cd, cs) =>
                    {
                        cd.Source = cs;
                        return cd;
                    }).ToList();

                    this.Cache.Set(cacheKey, list);
                }
            }

            return list.Take(count).ToList();
        }

        private bool IsCommentExisted(int id)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"SELECT 1 FROM [Comment] WITH(NOLOCK) WHERE ID=@ID";
                var para = new
                {
                    ID = id
                };

                return conn.Exist(sql, para);
            }
        }
    }
}
