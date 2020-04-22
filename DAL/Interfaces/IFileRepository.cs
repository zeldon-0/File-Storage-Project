using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
namespace DAL.Interfaces
{
    public interface IFileRepository : ISingleKeyRepository<File, Guid>
    {
        Task<File> GetFileById(Guid id);
        Task<IEnumerable<File>> GetUserFiles(int userId);
        Task<IEnumerable<File>> GetFolderFiles(Guid folderId);
        Task<IEnumerable<File>> GetSharedFiles(int userId);

    }
}
