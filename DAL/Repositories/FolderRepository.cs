using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    public class FolderRepository : ISingularRepository<Folder, Guid>
    {
        private FileStorageContext _context;
        public FolderRepository(FileStorageContext context)
        {
            _context = context;
        }

        public async Task<Folder> Create(Folder f)
        {
            if (f != null)
                await _context.Folders.AddAsync(f);
            await Save();
            return f;
        }

        public async Task Delete(Guid id)
        {
            Folder f = await _context.Folders.FindAsync(id);

            if (f != null)
                _context.Folders.Remove(f);
            await Save();

        }

        public async Task<IEnumerable<Folder>> GetAll()
        {
            return await _context.Folders.ToListAsync();
        }

        public async Task<Folder> GetByID(Guid id)
        {

            return await _context.Folders
                   .Include(f => f.Owner)
                   .Include(f => f.Subfolders)
                   .Include(f => f.Files)
                   .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Folder f)
        {
            if (f == null)
                return;

            Folder folder = await GetByID(f.Id);
            if (folder != null)
            {
                _context.Entry(folder).CurrentValues.SetValues(f);
                _context.Entry(folder).State = EntityState.Modified;
            }
            await Save();
        }
    }
}
