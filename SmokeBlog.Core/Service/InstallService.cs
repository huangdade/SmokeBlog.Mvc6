using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Runtime;
using SmokeBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Service
{
    public class InstallService : ServiceBase
    {
        private IApplicationEnvironment ApplicationEnvironment { get; set; }

        public InstallService(IConfiguration configuration, IApplicationEnvironment applicationEnvironment)
            : base(configuration)
        {
            this.ApplicationEnvironment = applicationEnvironment;
        }

        public bool NeedInstall()
        {
            bool nnedInstall = string.IsNullOrWhiteSpace(this.Configuration.Get("ConnectionString:Server")) || string.IsNullOrWhiteSpace(this.Configuration.Get("ConnectionString:Database"));

            return nnedInstall;
        }

        public OperationResult Install(string server, string database, string userID, string password)
        {
            if (string.IsNullOrWhiteSpace(server)
                    || string.IsNullOrWhiteSpace(database)
                    || string.IsNullOrWhiteSpace(userID)
                    || string.IsNullOrWhiteSpace(password)
                )
            {
                return OperationResult.ErrorResult("请填写必填信息");
            }
            else
            {
                try
                {
                    var data = new
                    {
                        ConnectionString = new
                        {
                            Server = server,
                            Database = database,
                            UserID = userID,
                            Password = password
                        }
                    };

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    string path = Path.Combine(ApplicationEnvironment.ApplicationBasePath, "config/database.json");

                    using (var writer = new StreamWriter(path))
                    {
                        writer.Write(json);
                    }

                    this.Configuration.Set("ConnectionString:Server", server);
                    this.Configuration.Set("ConnectionString:Database", database);
                    this.Configuration.Set("ConnectionString:UserID", userID);
                    this.Configuration.Set("ConnectionString:Password", password);

                    return OperationResult.SuccessResult();
                }
                catch (Exception ex)
                {
                    return OperationResult.ErrorResult(ex.Message);
                }
            }
        }
    }
}
