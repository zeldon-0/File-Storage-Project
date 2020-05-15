using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
namespace BLL.Interfaces
{
    public interface IFileService :IDisposable
    {
        Task<FileDTO> CreateAtRoot(FileDTO file, int userId);
        Task<FileDTO> GetFileById(Guid fileId);
        Task Delete(Guid id);
        Task Update(FileDTO file);
        Task<IEnumerable<FileDTO>> GetUserFiles(int userId);
        Task<IEnumerable<FileDTO>> GetFolderFiles(Guid folderId);
        Task<FileDTO> CreateAtFolder(FileDTO file, Guid folderId, int userId);
        Task MoveToFolder(Guid fileId, Guid folderId);
        Task MoveToRoot(Guid fileId);
        Task<FileDTO> CopyFile(Guid fileId);
    }
}
