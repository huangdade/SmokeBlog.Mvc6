using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models.Comment;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmokeBlog.Core.Extensions;

namespace SmokeBlog.Web.Controllers
{
    public class CommentController : Controller
    {
        private CommentService CommentService { get; set; }

        public CommentController(CommentService commentService)
        {
            this.CommentService = commentService;
        }

        [HttpGet("view/{articleID:int}/comments")]
        public IActionResult Query(int articleID)
        {
            var commentList = this.CommentService.QueryNestedByArticle(articleID);

            return this.View("CommentList", commentList);
        }

        [HttpPost("view/{articleID:int}/comments")]
        public IActionResult Add(AddCommentRequest model)
        {
            model.PostIP = this.Context.GetClientIP();

            var result = this.CommentService.Add(model);

            var or = new ObjectResult(result);
            return or;
        }
    }
}
