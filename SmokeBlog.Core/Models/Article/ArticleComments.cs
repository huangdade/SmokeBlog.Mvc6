using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Article
{
    public class ArticleComments
    {
        public int Total { get; set; }

        public int Pass { get; set; }

        public int Junk { get; set; }
    }
}
