using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SmokeBlog.Core.Extensions;

namespace SmokeBlog.Core.Models.Article
{
    public class ChangeStatusRequest
    {
        [Required]
        public string ID { get; set; }

        public Enums.ArticleStatus Status { get; set; }

        public int[] ArticleIDList
        {
            get
            {
                return this.ID.ToIntArray();
            }
        }
    }
}
