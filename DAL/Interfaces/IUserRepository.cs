using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using System.Threading.Tasks;
namespace DAL.Interfaces
{
    public interface IUserRepository : ISingleKeyRepository<User, int> 
    {
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetUsersByFileShare(Guid fileId);
        Task<IEnumerable<User>> GetUsersByFolderShare(Guid folderId);
    }
}
