using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmokeBlog.Core.Data;

namespace SmokeBlog.Core
{
    internal sealed class MapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<SmokeBlogContext.User, Models.User.UserModel>();
        }
    }
}
