using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.WebUtilities;
using SmokeBlog.Core.Models;
using SmokeBlog.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Controllers.Base
{
    [ValidateRequest]
    [TypeFilter(typeof(RequireLoginAttribute), Arguments = new object[] { RequireLoginAttribute.OutputTypes.Json })]
    public class ApiControllerBase : Controller
    {
        [NonAction]
        public IActionResult ApiResponse(OperationResult model)
        {
            return new ObjectResult(model);
        }

        [NonAction]
        public IActionResult BadRequest(string errorMessage = "错误的请求")
        {
            var model = OperationResult.ErrorResult(errorMessage);

            var result = new ObjectResult(model);
            result.StatusCode = StatusCodes.Status400BadRequest;

            return result;
        }

        [NonAction]
        public IActionResult FatalError(string errorMessage = "服务器发生错误")
        {
            var model = OperationResult.ErrorResult(errorMessage);

            var result = new ObjectResult(model);
            result.StatusCode = StatusCodes.Status500InternalServerError;

            return result;
        }
    }
}
