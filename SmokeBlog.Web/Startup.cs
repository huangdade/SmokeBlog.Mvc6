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
            services.AddMvc().ConfigureMvc(option=> 
            {
                var formatter = option.OutputFormatters.SingleOrDefault(t => t.Instance is JsonOutputFormatter);

                if (formatter != null)
                {
                    var jsonFormatter = (JsonOutputFormatter)formatter.Instance;
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
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
