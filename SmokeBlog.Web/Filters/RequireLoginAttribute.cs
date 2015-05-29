using Microsoft.AspNet.Mvc;
using SmokeBlog.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Internal;
using SmokeBlog.Core.Models;
using Microsoft.AspNet.WebUtilities;

namespace SmokeBlog.Web.Filters
{
    public class RequireLoginAttribute : AuthorizationFilterAttribute
    {
        public enum OutputTypes
        {
            Json = 1,
            Redirect = 2
        }

        public OutputTypes Output { get; set; }

        private ISecurityManager SecurityManager { get; set; }

        public RequireLoginAttribute(ISecurityManager securityManager, OutputTypes output)
        {
            this.SecurityManager = securityManager;
            this.Output = output;
        }

        public override void OnAuthorization(AuthorizationContext context)
        {            
            if (this.SecurityManager.IsAuthorized)
            {
                base.OnAuthorization(context);
            }
            else
            {
                this.HandleUnAuthorized(context);
            }
        }

        private void HandleUnAuthorized(AuthorizationContext context)
        {
            if (this.Output == OutputTypes.Json)
            {
                var response = new ObjectResult(OperationResult.ErrorResult("请登录"));
                response.StatusCode = StatusCodes.Status401Unauthorized;

                context.Result = response;
            }
            else
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
