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
        Task AddAccountToRole(string email, string role);
        Task RemoveFromRole(string email, string role);


    }
}
