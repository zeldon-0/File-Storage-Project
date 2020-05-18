using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Context.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(fi => fi.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
