using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Category;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Areas.Admin.Controllers
{
    [Route("api/category")]
    public class CategoryController : ApiControllerBase
    {
        private CategoryService CategoryService { get; set; }

        public CategoryController(CategoryService categoryService)
        {
            this.CategoryService = categoryService;
        }

        [HttpPost("add")]
        public IActionResult Add(AddCategoryRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.CategoryService.Add(model);

            return this.ApiResponse(result);
        }

        [HttpPost("edit")]
        public IActionResult Edit(EditCategoryRequest model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var result = this.CategoryService.Edit(model);

            return this.ApiResponse(result);
        }

        [HttpGet("all")]
        public IActionResult All()
        {
            var list = this.CategoryService.All();
            var result = OperationResult<List<NestedCategoryModel>>.SuccessResult(list);

            return this.ApiResponse(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var model = this.CategoryService.Get(id);
            var result = OperationResult<CategoryData>.SuccessResult(model);
            return this.ApiResponse(result);
        }
    }
}
