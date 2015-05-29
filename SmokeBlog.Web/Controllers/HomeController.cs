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
        public HomeController()
        {

        }

        [Route("")]
        [TypeFilter(typeof(RequireLoginAttribute), Arguments = new object[] { RequireLoginAttribute.OutputTypes.Json })]
        public IActionResult Index()
        {
            //Response.Cookies.Append("token", "w0tno2lr23c7g23haxmg5kewamyv4as1");

            //if (this.SecurityManager.IsAuthorized)
            //{
            //    return new ObjectResult(this.SecurityManager.LoginUser);
            //}
            //else
            //{
                return this.Content("No login");
            //}

            //var result = Core.Models.OperationResult.SuccessResult();

            //return result;
        }
    }
}
