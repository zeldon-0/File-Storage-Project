using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    public class FileShareRepository : ICompositeRepository<FileShare>
    {
        private FileStorageContext _context;
        public FileShareRepository(FileStorageContext context)
        {
            _context = context;
        }

        public async Task<FileShare> Create(FileShare fs)
        {
            if (fs != null)
                await _context.FileShares.AddAsync(fs);
            await Save();
            return fs;
        }

        public async Task Delete(Guid fileId, int userId)
        {
            FileShare fs = await _context.FileShares.FindAsync(new { fileId, userId });

            if (fs != null)
                _context.FileShares.Remove(fs);
            await Save();

        }

        public async Task<IEnumerable<FileShare>> GetAll()
        {
            return await _context.FileShares
                                 .ToListAsync();
        }

        public async Task<FileShare> GetByID(Guid fileId, int userId)
        {

            return await _context.FileShares
                   .FirstOrDefaultAsync(f => f.FileId == fileId && f.UserId == userId);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(FileShare fs)
        {
            if (fs == null)
                return;

            FileShare fileShare = await GetByID(fs.FileId, fs.UserId);
            if (fileShare != null)
            {
                _context.Entry(fileShare).CurrentValues.SetValues(fs);
                _context.Entry(fileShare).State = EntityState.Modified;
            }
            await Save();

        }
    }
}
