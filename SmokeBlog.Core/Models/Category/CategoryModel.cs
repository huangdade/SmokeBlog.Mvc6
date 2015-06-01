using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Category
{
    public class CategoryModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Articles { get; set; }

        public List<CategoryModel> Children { get; set; }
    }
}
