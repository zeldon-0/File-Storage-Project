using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Context.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Files)
                    .WithOne(fi => fi.Owner)
                    .HasForeignKey(fi => fi.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Folders)
                    .WithOne(fo => fo.Owner)
                    .HasForeignKey(fo => fo.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.FileShares)
                    .WithOne(fs => fs.User)
                    .HasForeignKey(fs => fs.UserId);

            builder.HasMany(u => u.FolderShares)
                    .WithOne(fs => fs.User)
                    .HasForeignKey(fs => fs.UserId);

            builder.Property(u => u.Email)
                    .IsRequired();
            builder.Property(u => u.UserName)
                    .IsRequired();

            PasswordHasher<User> hasher =
                new PasswordHasher<User>();

            builder.HasData(
                new User()
                {
                    Id = 1,
                    UserName = "TestUser",
                    NormalizedUserName = "TestUser".ToUpper(),
                    Email = "testUser@gmail.com",
                    NormalizedEmail = "testUser@gmail.com".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "TestUser1"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },

                new User()
                {
                    Id = 2,
                    UserName = "TestCorporate",
                    NormalizedUserName = "TestCorporate".ToUpper(),
                    Email = "corporate@gmail.com",
                    NormalizedEmail = "corporate@gmail.com".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "TestCorporate1"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },

                new User()
                {
                    Id = 3,
                    UserName = "TestAdmin",
                    NormalizedUserName = "TestAdmin".ToUpper(),
                    Email = "testAdmin@gmail.com",
                    NormalizedEmail = "testAdmin@gmail.com".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "TestAdmin1"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                }) ;
        }
    }
}
