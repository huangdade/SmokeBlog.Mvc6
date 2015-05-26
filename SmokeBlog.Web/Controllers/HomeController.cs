using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserService UserService { get; set; }

        private AuthService AuthService { get; set; }

        public HomeController(UserService userService, AuthService authService)
        {
            this.UserService = userService;
            this.AuthService = authService;
        }

        [Route("")]
        public object Index()
        {
            var result = this.AuthService.Login("admin", "111111");

            var request = new Core.Models.User.AddUserRequest
            {
                UserName = "admin",
                Password = "111111",
                Nickname = "管理员",
                Email = "5373827@qq.com"
            };

            //var result = this.UserService.Add(request);

            return result;
        }
    }
}
