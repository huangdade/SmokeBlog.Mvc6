using Microsoft.Framework.ConfigurationModel;
using SmokeBlog.Core.Data;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SmokeBlog.Core.Service
{
    public class CategoryService : ServiceBase
    {
        public CategoryService(IConfiguration configuration)
            : base(configuration)
        {

        }

        private List<CategoryData> GetCategoryList()
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"SELECT ID,ParentID,Name FROM [Category] WITH(NOLOCK)";

                return conn.Query<CategoryData>(sql).ToList();
            }
        }

        private bool CheckCategoryName(string name, int? id)
        {
            var list = this.GetCategoryList();

            var query = list.Where(t => t.Name == name);

            if (id.HasValue)
            {
                query = query.Where(t => t.ID == id.Value);
            }

            return query.Any() == false;
        }

        private bool CheckParentID(int parentID)
        {
            var list = this.GetCategoryList();

            return list.Any(t => t.ID == parentID && t.ParentID == null);
        }

        public OperationResult<int?> Add(AddCategoryRequest model)
        {
            if (!this.CheckCategoryName(model.Name, null))
            {
                return OperationResult<int?>.ErrorResult("分类名称重复");
            }
            if (model.ParentID.HasValue && !this.CheckParentID(model.ParentID.Value))
            {
                return OperationResult<int?>.ErrorResult("错误的上级分类");
            }

            using (var conn = this.OpenConnection())
            {
                string sql = @"
INSERT INTO [Category] ( Name, ParentID )
VALUES ( @Name, @ParentID );

SELECT @@IDENTITY;
";

                var para = new
                {
                    Name = model.Name,
                    ParentID = model.ParentID
                };

                var id = conn.ExecuteScalar<int>(sql, para);

                return OperationResult<int?>.SuccessResult(id);
            }
        }

        public OperationResult Edit(EditCategoryRequest model)
        {
            if (!this.CheckCategoryName(model.Name, model.ID))
            {
                return OperationResult<int?>.ErrorResult("分类名称重复");
            }
            if (model.ParentID.Value == model.ID.Value)
            {
                return OperationResult.ErrorResult("错误的上级分类");
            }
            if (model.ParentID.HasValue && !this.CheckParentID(model.ParentID.Value))
            {
                return OperationResult<int?>.ErrorResult("错误的上级分类");
            }

            using (var conn = this.OpenConnection())
            {
                string sql = @"
UPDATE TOP(1) [Category]
SET Name=@Name, ParentID=@ParentID
WHERE ID=@ID;
";

                var para = new
                {
                    model.ID,
                    model.Name,
                    model.ParentID
                };

                var rows = conn.Execute(sql, para);

                if (rows == 0)
                {
                    return OperationResult.ErrorResult("不存在的分类");
                }
                else
                {
                    return OperationResult.SuccessResult();
                }
            }
        }

        public List<NestedCategoryModel> All()
        {
            var list = this.GetCategoryList();

            var result = new List<NestedCategoryModel>();

            var parentList = list.Where(t => t.ParentID == null).ToList();

            foreach (var item in parentList)
            {
                var parent = new NestedCategoryModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Articles = 0,
                    Children = new List<NestedCategoryModel>()
                };

                var childrenList = list.Where(t => t.ParentID == item.ID).ToList();

                foreach (var item2 in childrenList)
                {
                    parent.Children.Add(new NestedCategoryModel
                    {
                        ID = item2.ID,
                        Name = item2.Name
                    });
                }

                result.Add(parent);
            }

            return result;
        }

        public CategoryData Get(int id)
        {
            var list = this.GetCategoryList();

            return list.SingleOrDefault(t => t.ID == id);
        }
    }
}
