using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Context.Configurations
{
    class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            builder.HasData(
                new IdentityRole<int>()
                {
                    Id = 1,
                    Name = "User",
                    NormalizedName = "USER"
                },

                new IdentityRole<int>()
                {
                    Id = 2,
                    Name = "Corporate",
                    NormalizedName = "CORPORATE"
                },

                new IdentityRole<int>()
                {
                    Id = 3,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
        }
        
    }
}
