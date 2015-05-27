using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Internal;
using SmokeBlog.Web.Controllers.Base;
using SmokeBlog.Core.Models;
using Microsoft.AspNet.WebUtilities;

namespace SmokeBlog.Web.Filters
{
    public class ValidateRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var model = OperationResult.ErrorResult("错误的请求");
                var result = new ObjectResult(model);
                result.StatusCode = StatusCodes.Status400BadRequest;

                context.Result = result;
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
