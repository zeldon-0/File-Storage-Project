using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using System.Linq;
namespace DAL.Repositories
{
    public class FileRepository: SingleKeyRepository<File, Guid>, IFileRepository
    {
        private FileStorageContext _context;
        public FileRepository(FileStorageContext context)
                : base(context)
        {
            _context = context;
        }


        public async Task<File> GetFileById(Guid id)
        {

            return await _context.Files
                   .AsNoTracking()
                   .Include(f => f.FileShares)
                   .Include(f => f.Owner)
                   .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<File>> GetFolderFiles(Guid folderID)
        {
            Folder folder = await _context.Folders
                    .Where(fo => fo.Id == folderID)
                    .Include(fo => fo.Files)
                    .FirstOrDefaultAsync();
            return folder.Files;
        }

        public async Task<IEnumerable<File>> GetUserFiles(int userId)
        {
            User user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Include( u => u.Files)
                    .ThenInclude(f => f.Folder)
                    .FirstOrDefaultAsync();
            return user.Files;
        }
        public async Task<IEnumerable<File>> GetSharedFiles(int userId)
        {
            List<FileShare> fileShares =
                await _context.FileShares
                      .Where(fs => fs.UserId == userId)
                      .Include(fs => fs.File)
                      .ToListAsync();
            return fileShares.Select(fs => fs.File);
        }
    }
}
