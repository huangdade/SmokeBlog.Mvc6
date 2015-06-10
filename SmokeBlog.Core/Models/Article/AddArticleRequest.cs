using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Article
{
    public class AddArticleRequest
    {
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string Summary { get; set; }

        public int UserID { get; set; }

        public string From { get; set; }

        public DateTime? PostDate { get; set; }

        public bool AllowComment { get; set; }

        public string Category { get; set; }

        public Enums.ArticleStatus? Status { get; set; }
    }
}
