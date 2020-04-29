using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService :IDisposable
    {
        Task<UserDTO> FindByEmail(string email);
        Task<UserDTO> FindById(int id);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetFileOwner(Guid fileId);
        Task<UserDTO> GetFolderOwner(Guid folderId);

    }
}
