﻿using System;
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
        public async Task<UserDTO> FindByEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException("A user with the provided email is yet to be registerd.");
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> FindById(int id)
        {
            User user = await _userManager.FindByIdAsync(Convert.ToString(id));
            if (user == null)
                throw new NotFoundException("A user with the provided id is yet to be registerd.");
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            IEnumerable<User> users = await _uow.Users.GetAll();
            return users.Select(u => _mapper.Map<UserDTO>(u));
        }

        public async Task<UserDTO> GetFileOwner(Guid fileId)
        {
            File file = await _uow.Files.GetFileById(fileId);
            if (file == null)
                throw new NotFoundException("The requested file does not exist");
            return _mapper.Map<UserDTO>(file.Owner);
        }

        public async Task<UserDTO> GetFolderOwner(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new NotFoundException("The requested folder does not exist");
            return _mapper.Map<UserDTO>(folder.Owner);
        }

        public async Task<IEnumerable<UserDTO>> GetFileShareUsers(Guid fileId)
        {
            File file = await _uow.Files.GetFileById(fileId);
            if (file == null)
                throw new NotFoundException("The requested file does not exist");
            IEnumerable<User> users = await _uow.Users.GetUsersByFileShare(fileId);
            return users.Select(u => _mapper.Map<UserDTO>(u));
        }

        public async Task<IEnumerable<UserDTO>> GetFolderShareUsers(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new NotFoundException("The requested folder does not exist");
            IEnumerable<User> users = await _uow.Users.GetUsersByFolderShare(folderId);
            return users.Select(u => _mapper.Map<UserDTO>(u));
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
