using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Runtime;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeBlog.Web.Middlewares
{
    public class InstallMiddleware
    {
        private bool IsInstalled { get; set; }

        private RequestDelegate Next { get; set; }

        private InstallService InstallService { get; set; }

        private IApplicationEnvironment ApplicationEnviroment { get; set; }

        public InstallMiddleware(RequestDelegate _next, InstallService installService, IApplicationEnvironment applicationEnviroment)
        {
            this.Next = _next;
            this.InstallService = installService;
            this.ApplicationEnviroment = applicationEnviroment;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsInstalled)
            {
                await Next.Invoke(context);
            }
            else
            {
                var needInstall = this.InstallService.NeedInstall();

                if (!needInstall)
                {
                    IsInstalled = true;
                    await Next.Invoke(context);
                }
                else
                {
                    await this.RenderInstallPage(context);
                }
            }
        }

        private async Task RenderInstallPage(HttpContext context)
        {
            if (context.Request.Method.ToLower() == "post")
            {
                var server = context.Request.Form["Server"];
                var database = context.Request.Form["Database"];
                var userID = context.Request.Form["UserID"];
                var password = context.Request.Form["Password"];

                var result = this.InstallService.Install(server, database, userID, password);

                if (result.Success)
                {
                    await this.RenderSuccessPage(context);
                }
                else
                {
                    await this.RenderPage(context, new InstallModel
                    {
                        Database = database,
                        ErrorMessage = result.ErrorMessage,
                        Password = password,
                        Server = server,
                        UserID = userID
                    });
                }
            }
            else
            {
                await this.RenderPage(context, new InstallModel());
            }
        }

        private async Task RenderPage(HttpContext context, InstallModel model)
        {
            string file = Path.Combine(ApplicationEnviroment.ApplicationBasePath, "Install.html");
            var html = File.ReadAllText(file);

            html = html.Replace("{Server}", model.Server ?? string.Empty);
            html = html.Replace("{Database}", model.Database ?? string.Empty);
            html = html.Replace("{UserID}", model.UserID ?? string.Empty);
            html = html.Replace("{Password}", model.Password ?? string.Empty);

            string errorMessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(model.ErrorMessage))
            {
                errorMessage = string.Format("<div class='alert alert-danger'>{0}</div>", model.ErrorMessage);
            }

            html = html.Replace("{ErrorMessage}", errorMessage);

            await this.RenderHtml(context, html);
        }

        private async Task RenderSuccessPage(HttpContext context)
        {
            string file = Path.Combine(ApplicationEnviroment.ApplicationBasePath, "InstallSuccess.html");
            var html = File.ReadAllText(file);

            await this.RenderHtml(context, html);
        }

        private async Task RenderHtml(HttpContext context, string html)
        {
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(html, Encoding.UTF8);
        }

        private class InstallModel
        {
            public string Server { get; set; }

            public string Database { get; set; }

            public string UserID { get; set; }

            public string Password { get; set; }

            public string ErrorMessage { get; set; }
        }
    }
}
