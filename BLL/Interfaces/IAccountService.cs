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
        Task<AuthenticationDTO> Authenticate(SignInDTO credentials);
        Task<UserDTO> Register(SignUpDTO user);
        Task<PrivateUserDTO> GetOwnInfo(string userId);
        Task Edit(UserDTO user);
        Task ChangePassword(SignInDTO credentials, string newPassword);
        Task Delete(string userId);
        Task AddAccountToRole(string userId, string role);
        Task RemoveAccountFromRole(string userId, string role);
        Task<AuthenticationDTO> UpdateAuthModel(string refreshToken, string userId);

    }
}
