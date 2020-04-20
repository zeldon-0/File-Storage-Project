using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;
namespace DAL.Context.Configurations
{

    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(fi => fi.Folder)
                    .WithMany(fo => fo.Files);
            builder.HasMany(fi => fi.FileShares)
                    .WithOne(fs => fs.File);
        }
    }
}
