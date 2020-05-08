using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;
using System.Threading.Tasks;
namespace BLL.Interfaces
{
    public interface ISharingService : IDisposable
    {
        Task ShareFolder(Guid folderId, string email);
        Task ShareFile(Guid fileId, string email);

        Task UnshareFolder(Guid folderId, string email);
        Task UnshareFile(Guid fileId, string email);

        Task<IEnumerable<FileDTO>> GetSharedFiles(int userId);
        Task<IEnumerable<FolderDTO>> GetSharedFolders(int userId);

        Task<IEnumerable<UserDTO>> GetSharedFileUserList(Guid fileId);
        Task<IEnumerable<UserDTO>> GetSharedFolderUserList(Guid folderId);



    }
}
