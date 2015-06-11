using SmokeBlog.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Comment
{
    public class CommentData
    {
        public int ID { get; set; }

        public string Content { get; set; }

        public int? ReplyTo { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }

        public DateTime PostDate { get; set; }

        public string PostIP { get; set; }

        public CommentStatus Status { get; set; }

        public bool NotifyOnReply { get; set; }

        public CommentSource Source { get; set; }
    }
}
