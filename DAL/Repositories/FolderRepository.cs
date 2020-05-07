using System;
using System.Data;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.Entities;
namespace DAL.Repositories
{
    public class FolderRepository : SingleKeyRepository<Folder, Guid>, IFolderRepository
    {
        private FileStorageContext _context;
        public FolderRepository(FileStorageContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<Folder> GetFileFolder(Guid fileId)
        {
            File file =  await _context.Files
                .Where(fi => fi.Id == fileId)
                .Include(fi => fi.Folder)
                .FirstOrDefaultAsync();
            return file.Folder;
        }

        public async Task<Folder> GetFolderById(Guid id)
        {
            return await _context.Folders
                   .AsNoTracking()
                   .Where(fo => fo.Id == id)
                   .Include(f => f.Owner)
                   .Include(f => f.Files)
                   .Include(f => f.Subfolders)
                   .Include(f => f.FolderShares)
                   .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Folder>> GetSharedFolders(int userId)
        {
            List<FolderShare> folderShares =
               await _context.FolderShares
               .Where(fs => fs.UserId == userId)
               .Include(fs => fs.Folder)
               .ToListAsync();
            return folderShares.Select(fs => fs.Folder);
        }

        public async Task<IEnumerable<Folder>> GetUserFolders(int userId)
        {
            User user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Folders)
                .ThenInclude(f => f.Parent)
                .FirstOrDefaultAsync();
            return user.Folders;
        }
    }
}
