using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;


namespace Middleware.Data_Seeding
{
    public static class UserSeed
    {
        public static void SeedUsers(UserManager<User> userManager)
        {
            User[] users = 
            {
                new User()
                {
                    UserName = "TestUser",
                    Email = "testUser@gmail.com"
                },
                new User()
                {
                    UserName = "TestCorporate",
                    Email = "corporate@gmail.com"
                },
                new User()
                {
                    UserName = "TestAdmin",
                    Email = "testAdmin@gmail.com"
                }

            };
            foreach (User user in users)
            {
                if (userManager.FindByEmailAsync(user.Email).Result == null
                    && userManager.FindByNameAsync(user.UserName).Result == null)
                {
                    userManager.CreateAsync( user, user.UserName + '1');
                    userManager.AddToRoleAsync(user, user.UserName.Substring(4));
                }
            }
        }
    }
}
