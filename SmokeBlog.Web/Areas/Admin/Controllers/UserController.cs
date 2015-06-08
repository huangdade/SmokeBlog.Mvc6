using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.User;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Areas.Admin.Controllers
{
    [Route("api/user")]
    public class UserController : ApiControllerBase
    {
        private UserService UserService { get; set; }

        public UserController(UserService userService)
        {
            this.UserService = userService;
        }

        [HttpGet("all")]
        public IActionResult All()
        {
            var list = this.UserService.All();

            var result = OperationResult<List<UserModel>>.SuccessResult(list);

            return this.ApiResponse(result);
        }

        [HttpPost("add")]
        public IActionResult Add(AddUserRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.UserService.Add(model);

            return this.ApiResponse(result);
        }

        [HttpPost("edit")]
        public IActionResult Edit(EditUserRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.UserService.Edit(model);

            return this.ApiResponse(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var user = this.UserService.Get(id);

            OperationResult<UserModel> result;

            if (user == null)
            {
                result = OperationResult<UserModel>.ErrorResult("不存在的用户");
            }
            else
            {
                result = OperationResult<UserModel>.SuccessResult(user);
            }            

            return this.ApiResponse(result);
        }
    }
}
