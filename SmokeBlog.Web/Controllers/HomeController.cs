using Microsoft.AspNet.Mvc;
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

        public HomeController(UserService userService)
        {
            this.UserService = userService;
        }

        [Route("")]
        public IActionResult Index()
        {
            //Response.Cookies.Delete("test");

            //this.UserService.Add(new Core.Models.User.AddUserRequest { UserName = "admin", Nickname = "管理员", Password = "111111", Email = "5373827@qq.com" });

            return this.Content("hehe");

            return this.HttpBadRequest();
        }
    }
}
