using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.User
{
    public class UserModel
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }

        public string Avatar { get; set; }
    }
}
