using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.ViewComponents
{
    public class CategoryWidget : ViewComponent
    {
        private CategoryService CategoryService { get; set; }

        public CategoryWidget(CategoryService categoryService)
        {
            this.CategoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var list = this.CategoryService.GetCategoryList().Where(t => t.Articles.Publish > 0).OrderBy(t => t.Name).ToList();

            return this.View(list);
        }
    }
}
