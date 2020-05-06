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
        Task Edit(UserDTO user, string email);
        Task ChangePassword(SignInDTO credentials, string newPassword);
        Task Delete(SignInDTO credentials);
        Task AddAccountToRole(string email, string role);
        Task RemoveFromRole(string email, string role);


    }
}
