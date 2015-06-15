using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.ViewComponents
{
    public class RecentArticlesWidget : ViewComponent
    {
        private ArticleService ArticleService { get; set; }

        public RecentArticlesWidget(ArticleService articleService)
        {
            this.ArticleService = articleService;
        }

        public IViewComponentResult Invoke()
        {
            var articles = this.ArticleService.GetLatestArticleList(10);

            return this.View(articles);
        }
    }
}
