using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;

namespace DAL.Context.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Files)
                    .WithOne(fi => fi.Owner)
                    .HasForeignKey(fi => fi.OwnerId);
            builder.HasMany(u => u.Folders)
                    .WithOne(fo => fo.Owner)
                    .HasForeignKey(fo => fo.OwnerId);

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

        }
    }
}
