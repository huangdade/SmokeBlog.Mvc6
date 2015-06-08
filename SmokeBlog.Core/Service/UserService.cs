using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SmokeBlog.Core.Service
{
    public class UserService : ServiceBase
    {
        public UserService(Microsoft.Framework.ConfigurationModel.IConfiguration configuration)
            : base(configuration)
        {

        }

        public List<UserModel> All()
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"SELECT * FROM [User]";

                var list = conn.Query<UserModel>(sql).ToList();

                return list;
            }
        }

        public OperationResult<int?> Add(AddUserRequest model)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
IF EXISTS(SELECT 1 FROM [User] WHERE UserName=@UserName)
BEGIN
    SELECT CONVERT(BIT, 1)
END
ELSE
BEGIN
    SELECT CONVERT(BIT, 0)
END
";
                var b = conn.ExecuteScalar<bool>(sql, new
                {
                    UserName = model.UserName
                });

                if (b)
                {
                    return OperationResult<int?>.ErrorResult("用户名已存在");
                }

                string password = Utility.MD5(model.Password);

                sql = @"
INSERT INTO [User] ( UserName, Password, Nickname, Email, CreateDate )
VALUES ( @UserName, @Password, @Nickname, @Email, GETDATE() );

SELECT @@IDENTITY;
";
                var para = new
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = password,
                    Nickname = model.Nickname
                };


                var id = conn.ExecuteScalar<int>(sql, para);

                return OperationResult<int?>.SuccessResult(id);
            }
        }

        public OperationResult Edit(EditUserRequest model)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
UPDATE TOP(1) [User]
SET Email=@Email, Nickname=@Nickname
WHERE ID=@ID
";

                var para = new
                {
                    ID = model.ID
                };

                var rows = conn.Execute(sql, para);

                if (rows > 0)
                {
                    return OperationResult.SuccessResult();
                }
                else
                {
                    return OperationResult.ErrorResult("不存在的用户");
                }
            }
        }

        public UserModel Get(int id)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT TOP 1 * FROM [User]
WHERE ID=@ID;
";
                var para = new
                {
                    ID = id
                };

                return conn.Query<UserModel>(sql, para).FirstOrDefault();
            }
        }

        public UserModel Get(string token)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
SELECT TOP 1 * FROM [User]
WHERE Token=@Token
";
                var para = new
                {
                    Token = token
                };

                return conn.Query<UserModel>(sql, para).FirstOrDefault();
            }
        }

        public OperationResult ChangePassword(int id, string oldPassword, string newPassword)
        {
            using (var conn = this.OpenConnection())
            {
                string sql = @"
UPDATE TOP(1) [User]
SET Password=@NewPassword
WHERE ID=@ID AND Password=@OldPassword;
";

                oldPassword = Utility.MD5(oldPassword);
                newPassword = Utility.MD5(newPassword);

                var para = new
                {
                    ID = id,
                    OldPassword = oldPassword,
                    NewPassword = newPassword
                };

                var rows = conn.Execute(sql, para);

                if (rows > 0)
                {
                    return OperationResult.SuccessResult();
                }
                else
                {
                    return OperationResult.ErrorResult("密码错误");
                }
            }
        }
    }
}
