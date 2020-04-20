using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    class FileRepository: ISingularRepository<File, Guid>
    {
        private FileStorageContext _context;
        public FileRepository(FileStorageContext context)
        {
            _context = context;
        }

        public async Task<File> Create(File f)
        {
            if (f != null)
                await  _context.Files.AddAsync(f);
            await Save();
            return f;
        }

        public async Task Delete(Guid id)
        {
            File f= await _context.Files.FindAsync(id);

            if (f != null)
                _context.Files.Remove(f);
            await Save();

        }

        public async Task<IEnumerable<File>> GetAll()
        {
            return await _context.Files.ToListAsync();
        }

        public async Task<File> GetByID(Guid id)
        {

            return await _context.Files
                   .Include(f => f.Owner)
                   .Include(f => f.Folder)
                   .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(File f)
        {
            if (f == null)
                return;

            File file = await GetByID(f.Id);
            if (file != null)
            {
                _context.Entry(file).CurrentValues.SetValues(f);
                _context.Entry(file).State = EntityState.Modified;
            }

        }
    }
}
