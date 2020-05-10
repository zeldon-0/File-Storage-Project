using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService :IDisposable
    {
        Task<PrivateUserDTO> FindByEmail(string email);
        Task<PrivateUserDTO> FindById(int id);
        Task EditUser(UserDTO user);
        Task DeleteUser(string email);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<IEnumerable<string>> GetUserRoles(string email);
    }
}
