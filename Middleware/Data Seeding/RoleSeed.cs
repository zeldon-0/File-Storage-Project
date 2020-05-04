using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;


namespace Middleware.Data_Seeding
{
    public static class RoleSeed
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "User", "Corporate", "Admin" };
            foreach(string role in roleNames)
            {
                if(roleManager.RoleExistsAsync(role).Result == false)
                {
                    roleManager.CreateAsync(
                        new IdentityRole()
                        {
                            Name = role,
                            NormalizedName = role.ToUpper()
                        }
                    ); 
                }
            }
        }
    }
}
