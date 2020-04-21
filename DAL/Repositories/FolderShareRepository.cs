using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    public class FolderShareRepository : ICompositeRepository<FolderShare>
    {
        private FileStorageContext _context;
        public FolderShareRepository(FileStorageContext context)
        {
            _context = context;
        }

        public async Task<FolderShare> Create(FolderShare fs)
        {
            if (fs != null)
                await _context.FolderShares.AddAsync(fs);
            await Save();
            return fs;
        }

        public async Task Delete(Guid folderId, int userId)
        {
            FolderShare fs = await _context.FolderShares.FindAsync(new { folderId, userId });

            if (fs != null)
                _context.FolderShares.Remove(fs);
            await Save();

        }

        public async Task<IEnumerable<FolderShare>> GetAll()
        {
            return await _context.FolderShares
                                 .ToListAsync();
        }

        public async Task<FolderShare> GetByID(Guid folderId, int userId)
        {

            return await _context.FolderShares
                   .FirstOrDefaultAsync(f => f.FolderId == folderId && f.UserId == userId);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(FolderShare fs)
        {
            if (fs == null)
                return;

            FolderShare folderShare = await GetByID(fs.FolderId, fs.UserId);
            if (folderShare != null)
            {
                _context.Entry(folderShare).CurrentValues.SetValues(fs);
                _context.Entry(folderShare).State = EntityState.Modified;
            }
            await Save();
        }
    }
}
