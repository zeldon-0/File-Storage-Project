using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    public class FileShareRepository : CompositeKeyRepository<FileShare>, IFileShareRepository
    {
        private FileStorageContext _context;
        public FileShareRepository(FileStorageContext context)
            :base(context)
        {

        }

        public async Task<bool> FileShareExists(Guid fileId, int userId)
        {
            FileShare fileShare = await
                _context.FileShares.FindAsync(new { fileId, userId });
            return (fileShare != null) ? true : false;
        }
    }
}
