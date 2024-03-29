﻿using System;
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
using Microsoft.Framework.Runtime;
using System.IO;

namespace SmokeBlog.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        private IApplicationEnvironment ApplicationEnviroment { get; set; }

        public Startup(IApplicationEnvironment enviroment)
        {
            this.Configuration = new Configuration()
                .AddJsonFile("config/database.json", true)
                .AddEnvironmentVariables();

#if DEBUG
            this.Configuration.Set("Debug", "True");
#else
            this.Configuration.Set("Debug", "False");
#endif
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInstance<IConfiguration>(this.Configuration);

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

            app.UseMiddleware<Middlewares.InstallMiddleware>();

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
}
