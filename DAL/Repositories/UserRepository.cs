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
    public class UserRepository : SingleKeyRepository<User, int>, IUserRepository
    {
        private FileStorageContext _context;
        public UserRepository(FileStorageContext context)
            :base(context)
        {

        }


        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);                        
        }

        public async Task<IEnumerable<User>> GetUsersByFileShare(Guid fileId)
        {
            IEnumerable<FileShare> fileShares =
                await _context.FileShares
                .Where(fs => fs.FileId == fileId)
                .Include(fs => fs.User)
                .ToListAsync();
            return fileShares.Select(fs => fs.User);

        }

        public async Task<IEnumerable<User>> GetUsersByFolderShare(Guid folderId)
        {
            IEnumerable<FolderShare> folderShares =
                await _context.FolderShares
                .Where(fs => fs.FolderId == folderId)
                .Include(fs => fs.User)
                .ToListAsync();
            return folderShares.Select(fs => fs.User);
        }

        
    }
}
