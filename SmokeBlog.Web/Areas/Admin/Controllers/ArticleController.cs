using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Article;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Areas.Admin.Controllers
{
    [Route("api/article")]
    public class ArticleController : ApiControllerBase
    {
        private ArticleService ArticleService { get; set; }

        public ArticleController(ArticleService articleService)
        {
            this.ArticleService = articleService;
        }

        [HttpGet("query")]
        public IActionResult Query(QueryArticleRequest request)
        {
            int total;
            var list = this.ArticleService.Query(request.PageIndex, request.PageSize, out total, request.Status, request.Keywords);

            var result = PagedOperationResult<ArticleModel>.SuccessResult(list, total);

            return this.ApiResponse(result);
        }
    }
}
