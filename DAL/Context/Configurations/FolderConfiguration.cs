using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;
namespace DAL.Context.Configurations
{

    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(fo => fo.Parent)
                    .WithMany(p => p.Subfolders);
            builder.HasMany(fo => fo.FolderShares)
                    .WithOne(fs => fs.Folder);

        }
    }
}