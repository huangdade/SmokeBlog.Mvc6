using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmokeBlog.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SmokeBlog.Core.Models.Comment
{
    public class ChangeStatusRequest
    {
        [Required]
        public string ID { get; set; }

        public Enums.ArticleStatus Status { get; set; }

        public int[] CommentIDList
        {
            get
            {
                return this.ID.ToIntArray();
            }
        }
    }
}
