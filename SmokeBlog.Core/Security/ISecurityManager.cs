using SmokeBlog.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Security
{
    public interface ISecurityManager
    {
        bool IsAuthorized { get; }

        UserModel LoginUser { get; }

        string Token { get; }
    }
}
