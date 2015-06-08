using Microsoft.Framework.ConfigurationModel;
using SmokeBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SmokeBlog.Core.Service
{
    public class AuthService : ServiceBase
    {
        public AuthService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public OperationResult<string> Login(string userName, string password)
        {
            using (var conn = this.OpenConnection())
            {
                password = Utility.MD5(password);

                string sql = @"SELECT TOP 1 ID FROM [User] WHERE UserName=@UserName AND Password=@Password";

                var para = new
                {
                    UserName = userName,
                    Password = password
                };

                var user = conn.Query(sql, para).FirstOrDefault();

                if (user == null)
                {
                    return OperationResult<string>.ErrorResult("用户名或密码错误");
                }

                string token = Utility.MD5(Guid.NewGuid().ToString());

                sql = @"UPDATE TOP(1) [User] SET Token=@Token WHERE ID =@ID";
                conn.Execute(sql, new
                {
                    ID = user.ID,
                    Token = token
                });

                return OperationResult<string>.SuccessResult(token);
            }
        }
    }
}
