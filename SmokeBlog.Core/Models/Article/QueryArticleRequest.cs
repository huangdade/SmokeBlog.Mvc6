using SmokeBlog.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Article
{
    public class QueryArticleRequest
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public ArticleStatus? Status { get; set; }

        public string Keywords { get; set; }
    }
}
