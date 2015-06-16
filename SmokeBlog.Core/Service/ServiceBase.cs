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
        protected IConfiguration Configuration { get; private set; }        

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
            var connectionString = string.Format("Server={0};Database={1};UID={2};PWD={3}",
                this.Configuration.Get("ConnectionString:Server"),
                this.Configuration.Get("ConnectionString:Database"),
                this.Configuration.Get("ConnectionString:UserID"),
                this.Configuration.Get("ConnectionString:Password")
                );

            var conn = new System.Data.SqlClient.SqlConnection(connectionString);
            conn.Open();

            return conn;
        }
    }
}
