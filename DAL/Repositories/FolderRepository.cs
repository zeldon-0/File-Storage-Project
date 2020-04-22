using System;
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

        }

        public async Task<Folder> GetFileFolder(Guid fileId)
        {
            File file =  await _context.Files.FindAsync(fileId);
            return file.Folder;
        }

        public async Task<Folder> GetFolderById(Guid id)
        {
            return await _context.Folders.FindAsync(id);
        }

        public async Task<IEnumerable<Folder>> GetSharedFolders(int userId)
        {
            List<FolderShare> folderShares =
               await _context.FolderShares
               .Where(fs => fs.UserId == userId).ToListAsync();
            return folderShares.Select(fs => fs.Folder);
        }

        public async Task<IEnumerable<Folder>> GetUserFolders(int userId)
        {
            User user = await _context.Users
                .FindAsync(userId);
            return user.Folders;
        }
    }
}
