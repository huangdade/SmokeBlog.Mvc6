using Microsoft.Framework.ConfigurationModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Service
{
    public abstract class ServiceBase
    {
        private IConfiguration Configuration { get; set; }

        protected ServiceBase(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            this.Configuration = configuration;
        }

        protected IDbConnection OpenConnection()
        {
            var connectionString = this.Configuration.Get("ConnectionStrings:SqlServer");

            var conn = new System.Data.SqlClient.SqlConnection(connectionString);
            conn.Open();

            return conn;
        }
    }
}
