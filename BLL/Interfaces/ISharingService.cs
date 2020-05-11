using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;
using System.Threading.Tasks;
namespace BLL.Interfaces
{
    public interface ISharingService : IDisposable
    {
        Task ShareFolder(Guid folderId, string userName);
        Task ShareFile(Guid fileId, string userName);

        Task UnshareFolder(Guid folderId, string userName);
        Task UnshareFile(Guid fileId, string userName);

        Task<IEnumerable<FileDTO>> GetSharedFiles(int userName);
        Task<IEnumerable<FolderDTO>> GetSharedFolders(int userName);

        Task<IEnumerable<UserDTO>> GetSharedFileUserList(Guid fileId);
        Task<IEnumerable<UserDTO>> GetSharedFolderUserList(Guid folderId);



    }
}
