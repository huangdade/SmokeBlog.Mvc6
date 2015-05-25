using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Data.Entity;

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
            services.AddMvc();
            services.AddEntityFramework().AddDbContext<Core.Data.SmokeBlogContext>(builder=> 
            {
                builder.UseSqlServer(Configuration.Get("ConnectionStrings:SqlServer"));
            }).AddSqlServer();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
