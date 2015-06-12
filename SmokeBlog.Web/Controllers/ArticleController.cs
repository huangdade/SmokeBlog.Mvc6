using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Controllers
{
    [Route("")]
    public class ArticleController : Controller
    {
        public IActionResult QueryByCategory()
        {
            return this.View();
        }

        public IActionResult QueryByAuthor()
        {
            return this.View();
        }
    }
}
