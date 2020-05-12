using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using System.IdentityModel.Tokens.Jwt;


namespace BLL.Interfaces
{
    public interface IAccountService : IDisposable
    {
        Task<JwtSecurityToken> Authenticate(SignInDTO credentials);
        Task<UserDTO> Register(SignUpDTO user);
        Task<PrivateUserDTO> GetOwnInfo(string email);
        Task Edit(UserDTO user);
        Task ChangePassword(SignInDTO credentials, string newPassword);
        Task Delete(SignInDTO credentials);
        Task AddAccountToRole(string email, string role);
        Task RemoveAccountFromRole(string email, string role);

    }
}
