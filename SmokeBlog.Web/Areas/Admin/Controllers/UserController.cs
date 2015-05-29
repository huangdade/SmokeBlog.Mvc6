using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.User;
using SmokeBlog.Core.Service;
using SmokeBlog.Web.Controllers.Base;
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
            var result = this.UserService.All();

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
            var result = this.UserService.Get(id);

            return this.ApiResponse(result);
        }
    }
}
