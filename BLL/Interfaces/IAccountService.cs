using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;


namespace BLL.Interfaces
{
    public interface IAccountService : IDisposable
    {
        Task<string> Authenticate(SignInDTO credentials);
        Task<UserDTO> Register(SignUpDTO user);
        Task Edit(UserDTO user);
        Task Delete(int userId);
        Task AddAccountToRole(int userId, string role);
        Task RemoveFromRole(int userId, string role);


    }
}
