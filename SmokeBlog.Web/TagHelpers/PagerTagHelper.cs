using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeBlog.Web.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {
        [Activate]
        protected IUrlHelper Url { get; set; }

        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PageIndex <= 0 || PageSize <= 0 || Total <= 0)
            {
                this.SetErrorMessage(output, "错误的分页参数");
                return;
            }

            var totalPage = this.Total / this.PageSize;
            if (this.Total % this.PageSize > 0)
            {
                totalPage++;
            }

            if (this.PageIndex > totalPage)
            {
                return;
            }

            var start = this.PageIndex - 4;
            if (start <= 0)
            {
                start = 1;
            }

            var end = start + 7;
            if (end > totalPage)
            {
                end = totalPage;
            }

            output.TagName = "ul";
            output.SelfClosing = false;
            output.Attributes.Add("class", "pagination");

            StringBuilder sb = new StringBuilder();

            if (this.PageIndex > 1)
            {
                sb.AppendFormat("<li><a href='{0}'>首页</a></li>", Url.RouteUrl(new { Page = 1 }));
                sb.AppendFormat("<li><a href='{0}'>上一页</a></li>", Url.RouteUrl(new { Page = this.PageIndex - 1 }));
            }
            for (var i = start; i <= end; i++)
            {
                if (this.PageIndex == i)
                {
                    sb.AppendFormat("<li class='active'><a href='javascript:void(0)'>{0}</a></li>", i);
                }
                else
                {
                    sb.AppendFormat("<li><a href='{0}'>{1}</a></li>", Url.RouteUrl(new { Page = i }), i);
                }                
            }
            if (this.PageIndex < totalPage)
            {
                sb.AppendFormat("<li><a href='{0}'>下一页</a></li>", Url.RouteUrl(new { Page = this.PageIndex + 1 }));
                sb.AppendFormat("<li><a href='{0}'>末页</a></li>", Url.RouteUrl(new { Page = totalPage }));
            }

            string html = string.Format("Total: {0}, Page: {1}, PageSize: {2}", Total, PageIndex, PageSize);

            output.Content.SetContent(sb.ToString());
            //base.Process(context, output);
        }

        private void SetErrorMessage(TagHelperOutput output, string message)
        {
            output.TagName = "div";
            output.Content.SetContent(message);
        }
    }
}
