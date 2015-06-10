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

        [HttpPost("add")]
        public IActionResult Add(AddArticleRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.ArticleService.Add(model);

            return this.ApiResponse(result);
        }

        [HttpPost("edit")]
        public IActionResult Edit(EditArticleRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.ArticleService.Edit(model);

            return this.ApiResponse(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var article = this.ArticleService.Get(id);

            OperationResult<ArticleModel> result;

            if (article == null)
            {
                result = OperationResult<ArticleModel>.ErrorResult("不存在的文章");
            }
            else
            {
                result = OperationResult<ArticleModel>.SuccessResult(article);
            }

            return this.ApiResponse(result);
        }
    }
}
