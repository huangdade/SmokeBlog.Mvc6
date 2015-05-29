using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmokeBlog.Core.Models.User;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;

namespace SmokeBlog.Core.Security
{
    public class DefaultSecurityManager : ISecurityManager
    {
        public bool IsAuthorized
        {
            get
            {
                return this.LoginUser != null;
            }
        }

        public UserModel LoginUser
        {
            get; private set;
        }

        public string Token
        {
            get; private set;
        }

        private HttpContext HttpContext { get; set; }

        private Service.UserService UserService { get; set; }

        public DefaultSecurityManager(IHttpContextAccessor accessor, Service.UserService userService)
        {
            this.HttpContext = accessor.HttpContext;
            this.UserService = userService;

            this.InitToken();
            this.InitUser();
        }

        private void InitToken()
        {
            string token = this.HttpContext.Request.Cookies["token"];

            this.Token = token;
        }

        private void InitUser()
        {
            if (this.Token != null)
            {
                var result = this.UserService.Get(this.Token);

                if (result.Success)
                {
                    this.LoginUser = result.Data;
                }
            }
        }
    }
}
