using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService :IDisposable
    {
        Task<PrivateUserDTO> FindByUserName(string userName);
        Task<PrivateUserDTO> FindById(int id);
        Task EditUser(UserDTO user);
        Task DeleteUser(string userName);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<IEnumerable<string>> GetUserRoles(string userName);
    }
}
