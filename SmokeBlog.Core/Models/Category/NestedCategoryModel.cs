using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Category
{
    public class NestedCategoryModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int PublishedArticles { get; set; }

        public int TotalArticles { get; set; }

        public List<NestedCategoryModel> Children { get; set; }
    }
}
