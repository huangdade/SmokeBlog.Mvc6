using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Runtime;
using SmokeBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SmokeBlog.Core.Service
{
    public class InstallService : ServiceBase
    {
        private IApplicationEnvironment ApplicationEnvironment { get; set; }

        private UserService UserService { get; set; }

        public InstallService(IConfiguration configuration, IApplicationEnvironment applicationEnvironment, UserService userService)
            : base(configuration)
        {
            this.ApplicationEnvironment = applicationEnvironment;
            this.UserService = userService;
        }

        public bool NeedInstall()
        {
            bool needInstall = string.IsNullOrWhiteSpace(this.Configuration.Get("ConnectionString:Server")) || string.IsNullOrWhiteSpace(this.Configuration.Get("ConnectionString:Database"));

            return needInstall;
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
                    this.SetConfiguration(server, database, userID, password);

                    this.InitDatabase();

                    this.AddAdministrator();

                    this.SaveConfiguration(server, database, userID, password);

                    return OperationResult.SuccessResult();
                }
                catch (Exception ex)
                {
                    this.ClearConfiguration();

                    return OperationResult.ErrorResult(ex.Message);
                }
            }
        }

        private void InitDatabase()
        {
            string sqlPath = Path.Combine(ApplicationEnvironment.ApplicationBasePath, "Install/database.sql");
            string sql = File.ReadAllText(sqlPath);

            using (var conn = this.OpenConnection())
            {
                conn.Execute(sql);
            }
        }

        private void AddAdministrator()
        {
            this.UserService.Add(new Models.User.AddUserRequest
            {
                Email = "admin@admin.com",
                UserName = "admin",
                Password = "admin",
                Nickname = "管理员"
            });
        }

        private void SaveConfiguration(string server, string database, string userID, string password)
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

            string directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName);

            using (var stream = File.Create(path))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }

        private void SetConfiguration(string server, string database, string userID, string password)
        {
            this.Configuration.Set("ConnectionString:Server", server);
            this.Configuration.Set("ConnectionString:Database", database);
            this.Configuration.Set("ConnectionString:UserID", userID);
            this.Configuration.Set("ConnectionString:Password", password);
        }

        private void ClearConfiguration()
        {
            this.Configuration.Set("ConnectionString:Server", string.Empty);
            this.Configuration.Set("ConnectionString:Database", string.Empty);
            this.Configuration.Set("ConnectionString:UserID", string.Empty);
            this.Configuration.Set("ConnectionString:Password", string.Empty);
        }
    }
}
