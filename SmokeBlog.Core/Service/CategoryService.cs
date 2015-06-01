using SmokeBlog.Core.Data;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Service
{
    public class CategoryService
    {
        private SmokeBlogContext DbContext { get; set; }

        public CategoryService(SmokeBlogContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public OperationResult<int?> Add(AddCategoryRequest model)
        {
            if (this.DbContext.Categories.Any(t => t.Name == model.Name))
            {
                return OperationResult<int?>.ErrorResult("分类名称重复");
            }

            if (model.ParentID.HasValue)
            {
                var parent = this.DbContext.Categories.SingleOrDefault(t => t.ID == model.ParentID.Value);

                if (parent == null)
                {
                    return OperationResult<int?>.ErrorResult("不存在的父级分类");
                }
                if (parent.ParentID.HasValue)
                {
                    return OperationResult<int?>.ErrorResult("只允许二级分类");
                }
            }

            var entity = new SmokeBlogContext.Category
            {
                ParentID = model.ParentID,
                Name = model.Name
            };
            DbContext.Add(entity);
            DbContext.SaveChanges();

            return OperationResult<int?>.SuccessResult(entity.ID);
        }

        public OperationResult Edit(EditCategoryRequest model)
        {
            if (this.DbContext.Categories.Any(t => t.ID != model.ID.Value && t.Name == model.Name))
            {
                return OperationResult.ErrorResult("分类名称重复");
            }
            if (model.ParentID.HasValue)
            {
                if (model.ParentID.Value == model.ID.Value)
                {
                    return OperationResult.ErrorResult("错误的父级分类");
                }

                var parent = this.DbContext.Categories.SingleOrDefault(t => t.ID == model.ParentID.Value);

                if (parent == null)
                {
                    return OperationResult.ErrorResult("不存在的父级分类");
                }
                if (parent.ParentID.HasValue)
                {
                    return OperationResult.ErrorResult("只允许二级分类");
                }
            }

            var entity = DbContext.Categories.SingleOrDefault(t => t.ID == model.ID);
            if (entity == null)
            {
                return OperationResult.ErrorResult("分类不存在或已被删除");
            }

            entity.Name = model.Name;
            entity.ParentID = model.ParentID;
            this.DbContext.SaveChanges();

            return OperationResult.SuccessResult();
        }

        public List<CategoryModel> All()
        {
            var list = this.DbContext.Categories.OrderBy(t => t.Name).ToList();

            var result = new List<CategoryModel>();

            var parentList = list.Where(t => t.ParentID == null).ToList();

            foreach (var item in parentList)
            {
                var parent = new CategoryModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Articles = 0,
                    Children = new List<CategoryModel>()
                };

                var childrenList = list.Where(t => t.ParentID == item.ID).ToList();

                foreach (var item2 in childrenList)
                {
                    parent.Children.Add(new CategoryModel
                    {
                        ID = item2.ID,
                        Name = item2.Name
                    });
                }

                result.Add(parent);
            }

            return result;
        }
    }
}
