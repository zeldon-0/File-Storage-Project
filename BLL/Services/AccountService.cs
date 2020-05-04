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


namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole<int>> _roleManager;

        public AccountService(IUnitOfWork uow, IMapper mapper, UserManager<User> userManager, 
             RoleManager<IdentityRole<int>> roleManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task AddAccountToRole(int userId, string role)
        {
            if (! await _roleManager.RoleExistsAsync(role))
            {
               await _roleManager.CreateAsync(
                    new IdentityRole<int>() 
                    { Name = role,
                      NormalizedName = role.ToUpper()});
            }
            User user = await _userManager.FindByIdAsync(Convert.ToString(userId));
            IdentityResult result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new Exception(errMessage.ToString());
            }
        }

        public async Task<string> Authenticate(SignInDTO credentials)
        {
            User user = await _userManager.FindByEmailAsync(credentials.Login)
                ?? await _userManager.FindByNameAsync(credentials.Login);
            if (user == null)
                throw new ArgumentException("A user with the provided login does not exist.");

            if (!await _userManager.CheckPasswordAsync(user, credentials.Password))
            {
                throw new ArgumentException("Failed to sign in with the provided login/password combination. Try again.");
            }

            SymmetricSecurityKey secretKey =
               new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JUGEMUjugemu123456789"));

            SigningCredentials signingCredentials =
                new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            IEnumerable<string> userRoles =
                await _userManager.GetRolesAsync(user);

            foreach (string role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            JwtSecurityToken tokenOptions =
                new JwtSecurityToken(
                    issuer: "http://localhost:5000",
                    audience: "http://localhost:5000",
                    claims: claims,
                    expires: DateTime.Now.AddHours(10),
                    signingCredentials: signingCredentials
                   );
            string tokenString =
                new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;

        }

        public async Task Delete(int userId)
        {
            User user = await _userManager.FindByIdAsync(Convert.ToString(userId));
            if (user == null)
                throw new ArgumentException("The specified role does not exist");

            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new Exception(errMessage.ToString());
            }
        }



        public async Task Edit(UserDTO user)
        {
            IdentityResult result = await _userManager.UpdateAsync(_mapper.Map<User>(user));
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new Exception(errMessage.ToString());
            }
        }

        public async Task<UserDTO> Register(SignUpDTO user)
        {
            if (await _userManager.FindByEmailAsync(user.Email) != null ||
                await _userManager.FindByNameAsync(user.UserName) != null)
                throw new ArgumentException("The user with these credentials already exists.");
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
                throw new Exception(errMessage.ToString());
            }

            User createdUser = await _userManager.FindByEmailAsync(user.Email);
            await AddAccountToRole(createdUser.Id, "User");

            return _mapper.Map<UserDTO>(createdUser);

        }

        public async Task RemoveFromRole(int userId, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new ArgumentException("The specified role does not exist");
            }
            User user = await _userManager.FindByIdAsync(Convert.ToString(userId));
            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role);

            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new Exception(errMessage.ToString());
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
