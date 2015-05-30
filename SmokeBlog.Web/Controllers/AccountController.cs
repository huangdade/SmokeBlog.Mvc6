using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Account;
using SmokeBlog.Core.Service;
using SmokeBlog.Web.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private UserService UserService { get; set; }

        private AuthService AuthService { get; set; }

        public AccountController(UserService userService, AuthService authService)
        {
            this.UserService = userService;
            this.AuthService = authService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var vm = new LoginViewModel();

            return this.View(vm);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var vm = new LoginViewModel();
            vm.Model = model;

            if (!ModelState.IsValid)
            {
                vm.Result = OperationResult.ErrorResult("错误的请求");
                return this.View(vm);
            }

            var result = this.AuthService.Login(model.UserName, model.Password);
            if (result.Success)
            {
                var option = new CookieOptions();
                if (model.RememberMe)
                {
                    option.Expires = DateTime.Now.AddMonths(6);
                }

                this.Response.Cookies.Append("token", result.Data, option);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
