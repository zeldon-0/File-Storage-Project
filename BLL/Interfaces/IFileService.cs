using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
namespace BLL.Interfaces
{
    public interface IFileService :IDisposable
    {
        Task<FileDTO> CreateAtRoot(FileDTO file);
        Task Delete(Guid id);
        Task Update(FileDTO file);
        Task<IEnumerable<FileDTO>> GetUserFiles(int userId);
        Task<IEnumerable<FileDTO>> GetFolderFiles(Guid folderId);
        Task<FileDTO> CreateAtFolder(FileDTO file, Guid folderId);
        Task MoveToFolder(Guid fileId, Guid folderId);
        Task<FileDTO> CopyFile(Guid fileId);
    }
}
