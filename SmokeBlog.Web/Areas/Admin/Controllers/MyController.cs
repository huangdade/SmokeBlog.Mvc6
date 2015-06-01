using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.My;
using SmokeBlog.Core.Models.User;
using SmokeBlog.Core.Security;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Areas.Admin.Controllers
{
    [Route("api/my")]
    public class MyController : ApiControllerBase
    {
        private ISecurityManager SecurityManager { get; set; }

        private UserService UserService { get; set; }

        public MyController(ISecurityManager securityManager, UserService userService)
        {
            this.SecurityManager = securityManager;
            this.UserService = userService;
        }

        [HttpGet("")]
        public IActionResult GetInfo()
        {
            var result = OperationResult<UserModel>.SuccessResult(this.SecurityManager.LoginUser);
            return this.ApiResponse(result);
        }

        [HttpPost("update")]
        public IActionResult UpdateInfo(UpdateInfoRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var request = new EditUserRequest
            {
                Email = model.Email,
                Nickname = model.Nickname,
                ID = this.SecurityManager.LoginUser.ID
            };

            var result = this.UserService.Edit(request);

            return this.ApiResponse(result);
        }

        [HttpPost("password")]
        public IActionResult ChangePassword(ChangePasswordRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.UserService.ChangePassword(this.SecurityManager.LoginUser.ID, model.OldPassword, model.NewPassword);

            return this.ApiResponse(result);
        }
    }
}
