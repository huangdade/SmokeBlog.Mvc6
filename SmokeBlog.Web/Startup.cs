using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Data.Entity;
using SmokeBlog.Core;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;

namespace SmokeBlog.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup()
        {
            this.Configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .AddUserSecrets();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Microsoft.AspNet.Authorization.IAuthorizationService, A>();
            services.AddMvc().ConfigureMvc(option=> 
            {
                var outputFormatter = option.OutputFormatters.SingleOrDefault(t => t.Instance is JsonOutputFormatter);

                if (outputFormatter != null)
                {
                    var jsonFormatter = (JsonOutputFormatter)outputFormatter.Instance;
                    jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }
            });
            services.AddEntityFramework().AddDbContext<Core.Data.SmokeBlogContext>(builder=> 
            {
                builder.UseSqlServer(Configuration.Get("ConnectionStrings:SqlServer"));
            }).AddSqlServer();
            services.AddCoreServices();

            //services.AddTransient<Filters.RequireLoginAttribute>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvc(route=>
            {
                route.MapRoute("Admin", "admin/{*path}", new
                {
                    controller = "Home",
                    area = "Admin",
                    action = "Index"
                });
            });
        }
    }

    public class A : Microsoft.AspNet.Authorization.IAuthorizationService
    {
        public bool Authorize(ClaimsPrincipal user, object resource, string policyName)
        {
            throw new NotImplementedException();
        }

        public bool Authorize(ClaimsPrincipal user, object resource, params IAuthorizationRequirement[] requirements)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, params IAuthorizationRequirement[] requirements)
        {
            throw new NotImplementedException();
        }
    }
}
