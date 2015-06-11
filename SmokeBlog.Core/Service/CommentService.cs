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

namespace SmokeBlog.Core.Service
{
    public class CommentService : ServiceBase
    {
        private ArticleService ArticleService { get; set; }

        public CommentService(IConfiguration configuration, ArticleService articleService)
            : base(configuration)
        {
            this.ArticleService = articleService;
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

                return OperationResult<int?>.SuccessResult(id);
            }
        }

        public List<CommentData> Query(QueryCommentRequest model, out int total)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT @Total=COUNT(1) FROM [Comment] WITH(NOLOCK) #strWhere#;

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
