using SmokeBlog.Core.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Extensions
{
    public static class ModelExtensions
    {
        public static NestedCommentModel ToNestedCommentModel(this CommentData entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new NestedCommentModel
            {
                Content = entity.Content,
                Email = entity.Email,
                ID = entity.ID,
                Nickname = entity.Nickname,
                NotifyOnReply = entity.NotifyOnReply,
                PostDate = entity.PostDate,
                PostIP = entity.PostIP,
                Status = entity.Status
            };

            return model;
        }
    }
}
