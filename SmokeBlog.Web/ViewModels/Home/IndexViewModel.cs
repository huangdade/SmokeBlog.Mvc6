using SmokeBlog.Core.Models.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public List<ArticleModel> ArticleList { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}
