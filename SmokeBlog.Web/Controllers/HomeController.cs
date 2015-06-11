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

namespace SmokeBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserService UserService { get; set; }

        private ArticleService ArticleService { get; set; }

        private CommentService CommentService { get; set; }

        public HomeController(UserService userService, ArticleService articleService, CommentService commentService)
        {
            this.UserService = userService;
            this.ArticleService = articleService;
            this.CommentService = commentService;
        }

        [Route("")]
        public IActionResult Index()
        {
            //Response.Cookies.Delete("test");

            var model2 = new AddCommentRequest
            {
                ArticleID = 2,
                Content = "回复下试试",
                Email = "t1@t.cn",
                Nickname = "昵称",
                PostIP = this.Context.GetClientIP(),
                NotifyOnReply = true
            };

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

            //var result = this.CommentService.Add(model2);

            //return new ObjectResult(result);

            //int total;

            //var list = this.ArticleService.Query(1, 10, out total, Core.Enums.ArticleStatus.Publish, "H");

            //var result = PagedOperationResult<ArticleModel>.SuccessResult(list, total);

            //return new ObjectResult(result);

            //this.UserService.Add(new Core.Models.User.AddUserRequest { UserName = "admin", Nickname = "管理员", Password = "111111", Email = "5373827@qq.com" });

            return this.Content("hehe");

            //return this.HttpBadRequest();
        }
    }
}
