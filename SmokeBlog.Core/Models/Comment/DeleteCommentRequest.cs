using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SmokeBlog.Core.Extensions;

namespace SmokeBlog.Core.Models.Comment
{
    public class DeleteCommentRequest
    {
        [Required]
        public string ID { get; set; }

        public int[] CommentIDList
        {
            get
            {
                return this.ID.ToIntArray();
            }
        }
    }
}
