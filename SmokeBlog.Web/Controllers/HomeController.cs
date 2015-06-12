using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Article;
using SmokeBlog.Core.Models.Comment;
using SmokeBlog.Core.Service;
using SmokeBlog.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmokeBlog.Core.Extensions;
using SmokeBlog.Web.ViewModels.Home;

namespace SmokeBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private ArticleService ArticleService { get; set; }

        public HomeController(ArticleService articleService)
        {
            this.ArticleService = articleService;
        }

        [HttpGet("")]
        public IActionResult Index(int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            var request = new QueryArticleRequest
            {
                PageIndex = page.Value,
                PageSize = 20,
                Status = Core.Enums.ArticleStatus.Publish
            };
            int total;

            var articleList = this.ArticleService.Query(request, out total);

            var vm = new ViewModels.Article.ArticleListViewModel
            {
                ArticleList = articleList,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Total = total
            };

            return this.View("~/Views/Article/ArticleList.cshtml", vm);
        }
    }
}
