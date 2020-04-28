using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
namespace BLL.Interfaces
{
    public interface IFolderService : IDisposable
    {
        Task<FileDTO> CreateAtRoot(FolderDTO folder);
        Task Delete(Guid id);
        Task Update(FolderDTO folder);
        Task<IEnumerable<FolderDTO>> GetUserFolders(int userId);
        Task<IEnumerable<FolderDTO>> GetFolderSubfolders(Guid folderId);
        Task<FileDTO> CreateAtFolder(FolderDTO folder, Guid folderId);
        Task<FileDTO> CopyToFolder(Guid subfolderId, Guid folderId);
    }
}
