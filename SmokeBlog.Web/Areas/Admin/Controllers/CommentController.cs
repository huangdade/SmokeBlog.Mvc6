using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Comment;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Areas.Admin.Controllers
{
    [Route("api/comment")]
    public class CommentController : ApiControllerBase
    {
        private CommentService CommentService { get; set; }

        public CommentController(CommentService commentService)
        {
            this.CommentService = commentService;
        }

        [HttpGet("query")]
        public IActionResult Query(QueryCommentRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            int total;

            var list = this.CommentService.Query(model, out total);

            var result = PagedOperationResult<CommentData>.SuccessResult(list, total);

            return this.ApiResponse(result);
        }

        [HttpPost("changestatus")]
        public IActionResult ChangeStatus(ChangeStatusRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.CommentService.ChangeStatus(model);

            return this.ApiResponse(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(DeleteCommentRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.CommentService.Delete(model);

            return this.ApiResponse(result);
        }

        [HttpPost("deletejunk")]
        public IActionResult DeleteJunk()
        {
            var result = this.CommentService.DeleteJunk();

            return this.ApiResponse(result);
        }
    }
}
