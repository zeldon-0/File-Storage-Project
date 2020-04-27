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

        }


        public async Task<File> GetFileById(Guid id)
        {

            return await _context.Files
                   .Include(f => f.Owner)
                   .Include(f => f.Folder)
                   .Include(f => f.FileShares)
                   .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<File>> GetFolderFiles(Guid folderID)
        {
            Folder folder = await _context.Folders
                   .FindAsync(folderID);
            return folder.Files;
        }

        public async Task<IEnumerable<File>> GetUserFiles(int userId)
        {
            User user = await _context.Users.FindAsync(userId);
            return user.Files;
        }
        public async Task<IEnumerable<File>> GetSharedFiles(int userId)
        {
            List<FileShare> fileShares =
                await _context.FileShares
                      .Include(fs => fs.File)
                      .Where(fs => fs.UserId == userId)
                      .ToListAsync();
            return fileShares.Select(fs => fs.File);
        }
    }
}
