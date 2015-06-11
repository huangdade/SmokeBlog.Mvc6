using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Article
{
    public class ArticleModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Summary { get; set; }

        public string From { get; set; }

        public DateTime PostDate { get; set; }

        public Enums.ArticleStatus Status { get; set; }

        public bool AllowComment { get; set; }

        public User.UserModel User { get; set; }

        public List<Category.CategoryModel> CategoryList { get; set; }

        public ArticleComments Comments { get; set; }
    }
}
