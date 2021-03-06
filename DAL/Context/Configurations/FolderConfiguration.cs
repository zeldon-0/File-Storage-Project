﻿using System;
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
                    .WithMany(fo => fo.Subfolders)
                    .HasForeignKey(fo => fo.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(fo => fo.Files)
                    .WithOne(fi => fi.Folder)
                    .HasForeignKey(fi => fi.FolderId)
                    .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(fo => fo.FolderShares)
                    .WithOne(fs => fs.Folder)
                    .HasForeignKey(fs => fs.FolderId); 

        }
    }
}