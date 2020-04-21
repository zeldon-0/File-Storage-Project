using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Context.Configurations;
namespace DAL.Context
{
    public class FileStorageContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FileShare> FileShares { get; set; }
        public DbSet<FolderShare> FolderShares { get; set; }

        public FileStorageContext(DbContextOptions<FileStorageContext> options) 
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);    

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FileConfiguration());
            modelBuilder.ApplyConfiguration(new FolderConfiguration());
            modelBuilder.ApplyConfiguration(new FolderShareConfiguration());
            modelBuilder.ApplyConfiguration(new FileShareConfiguration());


        }


    }
}
