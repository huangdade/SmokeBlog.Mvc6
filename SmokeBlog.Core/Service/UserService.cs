using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace SmokeBlog.Core.Service
{
    public class UserService
    {
        private Data.SmokeBlogContext DbContext { get; set; }

        public UserService(Data.SmokeBlogContext db)
        {
            this.DbContext = db;
        }

        public OperationResult<List<UserModel>> All()
        {
            var list = DbContext.Users.Project().To<UserModel>().ToList();
            return OperationResult<List<UserModel>>.SuccessResult(list);
        }

        public OperationResult<int?> Add(AddUserRequest model)
        {
            if (DbContext.Users.Any(t => t.UserName == model.UserName))
            {
                return OperationResult<int?>.ErrorResult("用户名已存在");
            }

            string salt = Utility.GetRandomString(32);
            string password = Encrypt.HMACSHA1Encryptor.Encrypt(model.Password, salt);

            var user = new Data.SmokeBlogContext.User
            {
                CreateDate = DateTime.Now,
                Email = model.Email,
                Nickname = model.Nickname,
                Salt = salt,
                Password = password,
                UserName = model.UserName
            };

            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            return OperationResult<int?>.SuccessResult(user.ID);
        }

        public OperationResult Edit(EditUserRequest model)
        {
            return null;
        }
    }
}
