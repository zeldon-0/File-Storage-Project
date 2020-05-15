using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
namespace BLL.Interfaces
{
    public interface IFolderService : IDisposable
    {
        Task<FolderDTO> CreateAtRoot(FolderDTO folder, int userId);
        Task<FolderDTO> GetFolderById(Guid folderId);
        Task Delete(Guid folderId);
        Task Update(FolderDTO folder);
        Task<IEnumerable<FolderDTO>> GetUserFolders(int userId);
        Task<IEnumerable<FolderDTO>> GetFolderSubfolders(Guid folderId);
        Task<FolderDTO> CreateAtFolder(FolderDTO folder, Guid folderId, int userId);
        Task MoveToFolder(Guid subfolderId, Guid parentFolderId);
        Task MoveToRoot(Guid folderId);
        Task<FolderDTO> CopyFolder(Guid folderId);
    }
}
