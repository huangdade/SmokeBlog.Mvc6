using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIP(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var feature = context.GetFeature<IHttpConnectionFeature>();

            if (feature == null)
            {
                return null;
            }

            return feature.RemoteIpAddress.ToString();
        }
    }
}
