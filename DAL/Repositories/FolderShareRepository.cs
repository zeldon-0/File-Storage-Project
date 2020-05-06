using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    public class FolderShareRepository : CompositeKeyRepository<FolderShare>, IFolderShareRepository
    {
        private FileStorageContext _context;
        public FolderShareRepository(FileStorageContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<bool> FolderShareExists(Guid fileId, int userId)
        {
            FolderShare folderShare = await
                _context.FolderShares.FindAsync(fileId, userId );
            return (folderShare != null) ? true : false;
        }
    }
}
