using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Comment
{
    public class AddCommentRequest
    {
        public int ArticleID { get; set; }

        public int? ReplyTo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        public string Content { get; set; }

        public string PostIP { get; set; }

        public bool NotifyOnReply { get; set; }
    }
}
