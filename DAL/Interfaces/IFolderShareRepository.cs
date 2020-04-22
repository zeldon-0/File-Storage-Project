using System;
using System.Threading.Tasks;
using DAL.Entities;
namespace DAL.Interfaces
{
    public interface IFolderShareRepository : ICompositeKeyRepository<FolderShare>
    {
        Task<bool> FolderShareExists(Guid folderId, int userId);
    }
}