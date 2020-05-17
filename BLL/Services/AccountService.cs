using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BLL.Exceptions;


namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly  IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountService(IUnitOfWork uow, IMapper mapper, UserManager<User> userManager, 
             RoleManager<IdentityRole<int>> roleManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task AddAccountToRole(string userName, string role)
        {
            if (! await _roleManager.RoleExistsAsync(role))
            {
               await _roleManager.CreateAsync(
                    new IdentityRole<int>() 
                    { Name = role,
                      NormalizedName = role.ToUpper()});
            }
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException("The requested user is not registered.");

            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(r => r == role))
                throw new BadRequestException("The user is already given the role's privileges.");

            IdentityResult result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }
        }

        public async Task<JwtSecurityToken> Authenticate(SignInDTO credentials)
        {
            User user = await _userManager.FindByEmailAsync(credentials.Login)
                ?? await _userManager.FindByNameAsync(credentials.Login);
            if (user == null)
                throw new NotFoundException("A user with the provided login does not exist.");

            if (!await _userManager.CheckPasswordAsync(user, credentials.Password))
            {
                throw new UnauthorizedException("Failed to sign in with the provided login/password combination. Try again.");
            }

            SymmetricSecurityKey secretKey =
               new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JUGEMUjugemu123456789"));

            SigningCredentials signingCredentials =
                new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            IEnumerable<string> userRoles =
                await _userManager.GetRolesAsync(user);

            foreach (string role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                    issuer: "http://localhost:5000",
                    audience: "http://localhost:5000",
                    claims: claims,
                    expires: DateTime.Now.AddHours(5),
                    signingCredentials: signingCredentials
                   );
            return token;

        }


        public async Task<PrivateUserDTO> GetOwnInfo(int id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new NotFoundException("The user with the provided id is not registered.");

            PrivateUserDTO info = new PrivateUserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return info;
        }

        public async Task Delete(SignInDTO credentials)
        {
            User user = await _userManager.FindByEmailAsync(credentials.Login)
                ?? await _userManager.FindByNameAsync(credentials.Login);
            if (user == null)
                throw new NotFoundException("The user with the provided login does not exist");

            if(!await _userManager.CheckPasswordAsync(user, credentials.Password))
            {
                throw new UnauthorizedException("Failed to auhenticate with the provided credentials.");
            }

            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }
        }


        public async Task Edit(UserDTO user)
        {
            User currentModel = await _userManager.FindByIdAsync(user.Id.ToString());
            if (currentModel == null)
                throw new NotFoundException("The user with the provided id does not exist");

            currentModel.Email = user.Email;
            currentModel.UserName = user.UserName;
            IdentityResult result = await _userManager.UpdateAsync(currentModel);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }
        }

        public async Task ChangePassword(SignInDTO credentials, string newPassword)
        {
            User user = await _userManager.FindByEmailAsync(credentials.Login);
            if (user == null)
                throw new NotFoundException("A user with the provided login does not exist.");

            if (!await _userManager.CheckPasswordAsync(user, credentials.Password))
            {
                throw new UnauthorizedException("Failed to auhenticate with the provided password.");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user,
                credentials.Password, newPassword);

            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }
        }

        public async Task<UserDTO> Register(SignUpDTO user)
        {
            if (await _userManager.FindByEmailAsync(user.Email) != null) 
                throw new NotFoundException("The user with this email already exists.");

            if (await _userManager.FindByNameAsync(user.UserName) != null)
                throw new NotFoundException("The user with this name already exists.");

            IdentityResult result = await _userManager.CreateAsync(new User()
                                        {
                                            UserName = user.UserName,
                                            NormalizedUserName = user.UserName.ToUpper(),
                                            Email = user.Email,
                                            NormalizedEmail = user.Email.ToUpper()
                                        },
                                         user.Password);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }

            await AddAccountToRole(user.UserName, "User");
            User createdUser = await _userManager.FindByEmailAsync(user.Email);

            return _mapper.Map<UserDTO>(createdUser);

        }

        public async Task RemoveAccountFromRole(string userName, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new NotFoundException("The specified role does not exist");
            }
            User user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                throw new NotFoundException("The user with the provided user name is not registered;");

            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any(r => r == role))
                throw new BadRequestException("The user does not have the role's privileges.");


            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (disposing)
                        _uow.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AccountService()
        {
            Dispose(false);
        }
    }
}
