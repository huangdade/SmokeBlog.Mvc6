using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Comment
{
    public class QueryCommentRequest
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public byte? Status { get; set; }

        public string Keywords { get; set; }
    }
}
