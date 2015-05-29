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
            var user = DbContext.Users.SingleOrDefault(t => t.ID == model.ID);
            if (user == null)
            {
                return OperationResult.ErrorResult("不存在的用户");
            }

            user.Email = model.Email;
            user.Nickname = model.Nickname;

            DbContext.SaveChanges();

            return OperationResult.SuccessResult();
        }

        public OperationResult<UserModel> Get(int id)
        {
            var user = DbContext.Users.Where(t => t.ID == id).ProjectTo<UserModel>().SingleOrDefault();

            if (user == null)
            {
                return OperationResult<UserModel>.ErrorResult("该用户不存在");
            }

            return OperationResult<UserModel>.SuccessResult(user);
        }

        public OperationResult<UserModel> Get(string token)
        {
            var user = DbContext.Users.Where(t => t.Token == token).ProjectTo<UserModel>().SingleOrDefault();

            if (user == null)
            {
                return OperationResult<UserModel>.ErrorResult("错误的token");
            }

            return OperationResult<UserModel>.SuccessResult(user);
        }

        public OperationResult ChangePassword(int id, string oldPassword, string newPassword)
        {
            var user = DbContext.Users.SingleOrDefault(t => t.ID == id);
            if (user == null)
            {
                return OperationResult.ErrorResult("该用户不存在");
            }

            oldPassword = Encrypt.HMACSHA1Encryptor.Encrypt(oldPassword, user.Salt);
            if (user.Password != oldPassword)
            {
                return OperationResult.ErrorResult("密码错误");
            }

            newPassword = Encrypt.HMACSHA1Encryptor.Encrypt(newPassword, user.Salt);
            user.Password = newPassword;
            DbContext.SaveChanges();

            return OperationResult.SuccessResult();
        }
    }
}
