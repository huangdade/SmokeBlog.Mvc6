using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Entity
{
    public class Article
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int Views { get; set; }

        public int? UserID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
