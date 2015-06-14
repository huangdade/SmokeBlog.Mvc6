using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Service;
using SmokeBlog.Web.ViewModels.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Controllers
{
    [Route("")]
    public class ArticleController : Controller
    {
        private ArticleService ArticleService { get; set; }

        public ArticleController(ArticleService articleService)
        {
            this.ArticleService = articleService;
        }

        [HttpGet("category/{id:int}")]
        public IActionResult QueryByCategory(int id, int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            int pageSize = 20, total;

            var articleList = this.ArticleService.QueryByCategory(id, page.Value, pageSize, out total);

            var vm = new ArticleListViewModel
            {
                ArticleList = articleList,
                PageIndex = page.Value,
                PageSize = pageSize,
                Total = total
            };

            return this.View("ArticleList", vm);
        }

        [HttpGet("author/{id:int}")]
        public IActionResult QueryByAuthor(int id, int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            int pageSize = 20, total;

            var articleList = this.ArticleService.QueryByAuthor(id, page.Value, pageSize, out total);

            var vm = new ArticleListViewModel
            {
                ArticleList = articleList,
                PageIndex = page.Value,
                PageSize = pageSize,
                Total = total
            };

            return this.View("ArticleList", vm);
        }

        [Route("view/{id:int}")]
        public IActionResult ViewArticle(int id)
        {
            var article = this.ArticleService.Get(id);

            if (article == null)
            {
                return this.HttpNotFound();
            }

            string nickname = this.Request.Cookies.Get("comment_nickname");
            string email = this.Request.Cookies.Get("comment_email");

            ViewBag.Comment_Nickname = nickname;
            ViewBag.Comment_Email = email;

            return this.View(article);
        }
    }
}
