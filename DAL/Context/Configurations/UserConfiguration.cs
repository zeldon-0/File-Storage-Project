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
                    .WithOne(fi => fi.Owner);
            builder.HasMany(u => u.Folders)
                    .WithOne(fo => fo.Owner);
            builder.HasMany(u => u.FileShares)
                    .WithOne(sf => sf.User);
            builder.HasMany(u => u.FolderShares)
                    .WithOne(sf => sf.User);

            builder.Property(u => u.Email)
                    .IsRequired();
            builder.Property(u => u.UserName)
                    .IsRequired();
        }
    }
}
