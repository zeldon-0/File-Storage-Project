using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;
namespace DAL.Context.Configurations
{
    public class FileShareConfiguration : IEntityTypeConfiguration<FileShare>
    {
        public void Configure(EntityTypeBuilder<FileShare> builder)
        {
            builder.HasKey(fs => new 
                            { 
                                fs.FileId,
                                fs.UserId
                            });
            builder.Ignore(fs => fs.ResourceId);
        }
    }
}
