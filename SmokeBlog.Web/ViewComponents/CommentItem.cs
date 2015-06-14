using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.ViewComponents
{
    public class CommentItem : ViewComponent
    {
        public IViewComponentResult Invoke(CommentData comment)
        {
            return this.View(comment);
        }
    }
}
