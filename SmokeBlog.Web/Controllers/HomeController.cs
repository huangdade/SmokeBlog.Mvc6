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
        [Route("")]
        public object Index()
        {
            var settings = this.Resolver.GetService(typeof(Newtonsoft.Json.JsonSerializerSettings));

            var result = Core.Models.OperationResult.SuccessResult();

            return result;
        }
    }
}
