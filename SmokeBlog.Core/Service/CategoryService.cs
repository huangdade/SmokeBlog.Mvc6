﻿using Microsoft.Framework.ConfigurationModel;
using SmokeBlog.Core.Data;
using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SmokeBlog.Core.Cache;

namespace SmokeBlog.Core.Service
{
    public class CategoryService : ServiceBase
    {
        private ICache Cache { get; set; }

        public CategoryService(IConfiguration configuration, Cache.ICache cache)
            : base(configuration)
        {
            this.Cache = cache;
        }

        public List<CategoryData> GetCategoryList()
        {
            string cacheKey = Helpers.CacheKeyHelper.GetCategoryListCacheKey();

            var list = this.Cache.Get<List<CategoryData>>(cacheKey, ()=> 
            {
                using (var conn = this.OpenConnection())
                {
                    string sql = @"
;WITH tb AS
(
	SELECT 
	CategoryID AS ID,
	SUM(CASE B.Status WHEN 2 THEN 1 ELSE 0 END) AS Publish,
    SUM(CASE B.Status WHEN 0 THEN 1 ELSE 0 END) AS [Delete],
	COUNT(1) AS Total
	FROM CategoryArticle A WITH(NOLOCK)
	JOIN Article B WITH(NOLOCK) ON A.ArticleID = B.ID
	GROUP BY A.CategoryID
)
SELECT 
A.ID, A.Name, A.ParentID, B.ID, B.Publish, B.Total, B.[Delete]
FROM Category A WITH(NOLOCK)
left JOIN tb B ON A.ID = B.ID
";

                    return conn.Query<CategoryData, CategoryArticles, CategoryData>(sql, (category, articles) =>
                    {
                        category.Articles = articles;
                        return category;
                    }).ToList();
                }
            });

            return list;
        }

        public void ClearCache()
        {
            string cacheKey = Helpers.CacheKeyHelper.GetCategoryListCacheKey();

            this.Cache.Remove(cacheKey);
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

                this.ClearCache();

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
            if (this.GetCategoryList().Any(t => t.ParentID == model.ID.Value))
            {
                return OperationResult<int?>.ErrorResult("只允许二级分类");
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
                    this.ClearCache();

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
                    Articles = item.Articles,
                    Children = new List<NestedCategoryModel>()
                };

                var childrenList = list.Where(t => t.ParentID == item.ID).ToList();

                foreach (var item2 in childrenList)
                {
                    parent.Children.Add(new NestedCategoryModel
                    {
                        ID = item2.ID,
                        Name = item2.Name,
                        Articles = item2.Articles
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

        public OperationResult Delete(DeleteCategoryRequest model)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
UPDATE [Category] SET ParentID=NULL WHERE ParentID IN @IDs;

DELETE FROM [Category] WHERE ID IN @IDs;

DELETE FROM [CategoryArticle] WHERE CategoryID IN @IDs;
";

                var para = new
                {
                    IDs = model.CategoryIDList
                };

                var rows = conn.Execute(sql, para);

                if (rows == 0)
                {
                    return OperationResult.ErrorResult("不存在的分类");
                }

                this.ClearCache();
                return OperationResult.SuccessResult();
            }
        }
    }
}
