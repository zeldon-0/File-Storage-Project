using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DAL.Context;
using DAL.Entities;
namespace Middleware.Configuration_Extension
{
    public static class ContextConfiguration
    {
        public static void ConfigureDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FileStorageContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FileStorageContext"),
                b => b.MigrationsAssembly("WebAPI")));

            services.AddIdentity<User, IdentityRole<int>>(options =>
               {
                   options.Password.RequiredLength = 8;
                   options.Password.RequireNonAlphanumeric = false;
                   options.User.RequireUniqueEmail = true;
               }
               )
            .AddEntityFrameworkStores<FileStorageContext>();      


        }
    }
}
