using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Comment
{
    public class NestedCommentModel
    {
        public int ID { get; set; }

        public string Content { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }

        public DateTime PostDate { get; set; }

        public string PostIP { get; set; }

        public Enums.CommentStatus Status { get; set; }

        public bool NotifyOnReply { get; set; }

        public List<NestedCommentModel> Replies { get; set; }
    }
}
