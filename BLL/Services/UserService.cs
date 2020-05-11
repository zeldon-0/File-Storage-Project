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
using BLL.Exceptions;
namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork uow, 
            IMapper mapper, UserManager<User> userManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<PrivateUserDTO> FindByUserName(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException("A user with the provided user name is yet to be registerd.");
            PrivateUserDTO info = new PrivateUserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return info;

        }

        public async Task<PrivateUserDTO> FindById(int id)
        {
            User user = await _userManager.FindByIdAsync(Convert.ToString(id));
            if (user == null)
                throw new NotFoundException("A user with the provided id is yet to be registerd.");
            PrivateUserDTO info = new PrivateUserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return info;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            IEnumerable<User> users = await _uow.Users.GetAll();
            return users.Select(u => _mapper.Map<UserDTO>(u));
        }


        public async Task EditUser(UserDTO user)
        {
            User currentUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (currentUser == null)
                throw new NotFoundException("Could not find the user corresponding to the provided model.");

            currentUser.Email = user.Email;
            currentUser.UserName = user.UserName;
            IdentityResult result = await _userManager.UpdateAsync(currentUser);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }
        }

        public async Task DeleteUser(string userName)
        {
            User currentUser = await _userManager.FindByNameAsync(userName);
            if (currentUser == null)
                throw new NotFoundException("Could bot find the user corresponding to the provided email.");

            IdentityResult result = await _userManager.DeleteAsync(currentUser);
            if (!result.Succeeded)
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                    errMessage.Append($"{err.Code}  {err.Description}/n");
                throw new ServerErrorException(errMessage.ToString());
            }
        }

        public async Task<IEnumerable<string>> GetUserRoles(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException("The user with the provided user name is not registered.");
            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

            return roles;
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


        ~UserService()
        {
            Dispose(false);
        }
    }
}
