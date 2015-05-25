using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(Core.Data.SmokeBlogContext db)
        {
            var user = db.Users.FirstOrDefault();
        }

        [Route("")]
        public IActionResult Index()
        {
            return this.Content("Hello world");
        }
    }
}
