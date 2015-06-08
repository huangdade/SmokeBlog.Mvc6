using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Article;
using SmokeBlog.Core.Service;
using SmokeBlog.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserService UserService { get; set; }

        private ArticleService ArticleService { get; set; }

        public HomeController(UserService userService, ArticleService articleService)
        {
            this.UserService = userService;
            this.ArticleService = articleService;
        }

        [Route("")]
        public IActionResult Index()
        {
            //Response.Cookies.Delete("test");

            var model = new AddArticleRequest
            {
                AllowComment = true,
                Category = "1,2",
                Content = "人类的一小步,地球的一大步",
                From = null,
                PostDate = DateTime.Now,
                Status = Core.Enums.ArticleStatus.Publish,
                Summary = null,
                Title = "Hello World!",
                UserID = 1
            };

            int total;

            var list = this.ArticleService.Query(1, 10, out total, Core.Enums.ArticleStatus.Publish, "H");

            var result = PagedOperationResult<ArticleModel>.SuccessResult(list, total);

            return new ObjectResult(result);

            //this.UserService.Add(new Core.Models.User.AddUserRequest { UserName = "admin", Nickname = "管理员", Password = "111111", Email = "5373827@qq.com" });

            //return this.Content("hehe");

            //return this.HttpBadRequest();
        }
    }
}
