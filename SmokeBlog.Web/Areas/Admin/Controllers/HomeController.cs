using Microsoft.AspNet.Mvc;
using SmokeBlog.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [TypeFilter(typeof(RequireLoginAttribute), Arguments = new object[] { RequireLoginAttribute.OutputTypes.Redirect })]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
