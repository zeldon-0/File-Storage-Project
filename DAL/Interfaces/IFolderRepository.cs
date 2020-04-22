using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
namespace DAL.Interfaces
{
    interface IFolderRepository : ISingleKeyRepository<Folder, Guid>
    {
        Task<Folder> GetFolderById(Guid id);
        Task<IEnumerable<Folder>> GetUserFolders(int userId);
        Task<Folder> GetFileFolder(Guid fileId);
        Task<IEnumerable<Folder>> GetSharedFolders(int userId);
    }
}
