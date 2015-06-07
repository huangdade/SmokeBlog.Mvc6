using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Data
{
    public partial class SmokeBlogContext
    {
        public class User
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Salt { get; set; }

            public string Password { get; set; }

            public string Email { get; set; }

            public string Nickname { get; set; }

            public string Avatar { get; set; }

            public DateTime CreateDate { get; set; }

            public string Token { get; set; }
        }

        public class Category
        {
            public int ID { get; set; }

            public int? ParentID { get; set; }

            public string Name { get; set; }
        }

        public class Article
        {
            public int ID { get; set; }

            public string Title { get; set; }

            public string Content { get; set; }

            public string Summary { get; set; }

            public int UserID { get; set; }

            public string From { get; set; }

            public DateTime PostDate { get; set; }

            public byte Status { get; set; }

            public bool AllowComment { get; set; }

            public User User { get; set; }

            public ICollection<Category> Categories { get; set; }
        }
    }
}
