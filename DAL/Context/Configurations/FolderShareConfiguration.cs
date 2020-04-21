using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;
namespace DAL.Context.Configurations
{
    public class FolderShareConfiguration : IEntityTypeConfiguration<FolderShare>
    {
        public void Configure(EntityTypeBuilder<FolderShare> builder)
        {
            builder.HasKey(fs => new
                            {
                                fs.FolderId,
                                fs.UserId
                            });
        }
    }
}
