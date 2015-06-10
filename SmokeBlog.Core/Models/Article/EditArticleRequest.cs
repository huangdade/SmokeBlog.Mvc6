using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Article
{
    public class EditArticleRequest : AddArticleRequest
    {
        [Required]
        public int? ID { get; set; }
    }
}
