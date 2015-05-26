using SmokeBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Service
{
    public class AuthService
    {
        private Data.SmokeBlogContext DbContext { get; set; }

        public AuthService(Data.SmokeBlogContext db)
        {
            this.DbContext = db;
        }

        public OperationResult<string> Login(string userName, string password)
        {            
            var user = this.DbContext.Users.SingleOrDefault(t => t.UserName == userName);

            if (user == null)
            {
                return OperationResult<string>.ErrorResult("用户不存在");
            }

            password = Encrypt.HMACSHA1Encryptor.Encrypt(password, user.Salt);
            if (password != user.Password)
            {
                return OperationResult<string>.ErrorResult("密码错误");
            }

            string token = Utility.GetRandomString(32);

            user.Token = token;
            DbContext.SaveChanges();

            return OperationResult<string>.SuccessResult(token);
        }
    }
}
