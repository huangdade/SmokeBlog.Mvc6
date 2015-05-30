using SmokeBlog.Core.Models;
using SmokeBlog.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        public LoginModel Model { get; set; }

        public OperationResult Result { get; set; }
    }
}
