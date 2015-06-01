﻿using Microsoft.Framework.DependencyInjection;
using SmokeBlog.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddTransient<UserService>()
                .AddTransient<AuthService>()
                .AddTransient<CategoryService>()
                .AddScoped<Security.ISecurityManager, Security.DefaultSecurityManager>();

            MapperConfig.Configure();
        }
    }
}
