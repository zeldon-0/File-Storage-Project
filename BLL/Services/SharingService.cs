using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;

namespace BLL.Services
{
    public class SharingService : ISharingService
    {
        private UserManager<User> _userManager;
        private IUnitOfWork _context;
        private IMapper _mapper;

        public SharingService(UserManager<User> userManager, 
               IUnitOfWork context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<FileDTO>> GetSharedFiles(int userId)
        {
            IEnumerable<File> files = await _context.Files.GetSharedFiles(userId);
            return files.Select(f => _mapper.Map<FileDTO>(f));
        }

        public async Task<IEnumerable<UserDTO>> GetSharedFileUserList(Guid fileId)
        {
            IEnumerable<User> users =  await _context.Users.GetUsersByFileShare(fileId);
            return users.Select(u => _mapper.Map<UserDTO>(u));
        }

        public async Task<IEnumerable<FolderDTO>> GetSharedFolders(int userId)
        {
            IEnumerable<Folder> folders = await _context.Folders.GetSharedFolders(userId);
            return folders.Select(f => _mapper.Map<FolderDTO>(f));
        }

        public async Task<IEnumerable<UserDTO>> GetSharedFolderUserList(Guid folderId)
        {
            IEnumerable<User> users = await _context.Users.GetUsersByFolderShare(folderId);
            return users.Select(u => _mapper.Map<UserDTO>(u));
        }

        public async Task ShareFile(Guid fileId, int userId)
        {
            File file = await _context.Files.GetFileById(fileId);
            if (file == null)
                throw new ArgumentException("The file with the provided Id does not exist");
            User user = await _context.Users.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("The user with the provided Id does not exist");
            await _context.FileShares.Create(
                new FileShare() { FileId = fileId, UserId = userId});
        }

        public async Task ShareFile(Guid fileId, string email)
        {
            File file = await _context.Files.GetFileById(fileId);
            if (file == null)
                throw new ArgumentException("The file with the provided Id does not exist");
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("The user with the provided Id does not exist");
            await _context.FileShares.Create(
                new FileShare() { FileId = fileId, UserId = user.Id });
        }

        public async Task ShareFolder(Guid folderId, int userId)
        {
            Folder folder = await _context.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new ArgumentException("The file with the provided Id does not exist");
            User user = await _context.Users.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("The user with the provided Id does not exist");
            await _context.FolderShares.Create(
                new FolderShare() { FolderId = folderId, UserId = userId });
            await ShareFolderSubfolders(folderId, userId);
        }

        public async Task ShareFolder(Guid folderId, string email)
        {
            Folder folder = await _context.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new ArgumentException("The file with the provided Id does not exist");
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("The user with the provided Id does not exist");
            //await _context.FolderShares.Create(
            //    new FolderShare() { FolderId = folderId, UserId = user.Id });
            await ShareFolderSubfolders(folderId, user.Id);
        }

        public async Task UnshareFile(Guid fileId, int userId)
        {
            bool fileShareExists = await _context.FileShares.FileShareExists(fileId, userId);
            if (!fileShareExists)
                throw new ArgumentException("The file is not shared with the provided user");
            else
            {
                await _context.FileShares.Delete(fileId, userId);
            }
        }

        public async Task UnshareFile(Guid fileId, string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("The user with the provided email is not yet registered.");
            else
                await UnshareFile(fileId, user.Id);
        }

        public async Task UnshareFolder(Guid folderId, int userId)
        {
            bool folderShareExists = await _context.FolderShares.FolderShareExists(folderId, userId);
            if (!folderShareExists)
                throw new ArgumentException("The folder is not shared with the provided user");
            else
            {
                await _context.FolderShares.Delete(folderId, userId);
            }
        }

        public async Task UnshareFolder(Guid folderId, string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("The user with the provided email is not yet registered.");
            else
                await UnshareFile(folderId, user.Id);
        }

        // Iterate over every child folder and share them 
        private async Task ShareFolderSubfolders(Guid folderId, int userId)
        {
            Folder folder = await _context.Folders.GetFolderById(folderId);
            foreach (Folder subfolder in folder.Subfolders)
            {
                await ShareFolderSubfolders(subfolder.Id, userId);
                await ShareFolderFiles(subfolder.Id, userId);
            }
            await ShareFolderFiles(folderId, userId);
            if (!(await _context.FolderShares.FolderShareExists(folderId, userId)))
                await _context.FolderShares.Create(
                    new FolderShare() { FolderId = folderId, UserId = userId });

        }
        // Iterate over every file in the given folder
        private async Task ShareFolderFiles(Guid folderId, int userId)
        {
            Folder folder = await _context.Folders.GetFolderById(folderId);
            foreach (File file  in folder.Files)
            {
                if(!(await _context.FileShares.FileShareExists(file.Id, userId)))
                    await _context.FileShares.Create(
                        new FileShare() { FileId = file.Id, UserId = userId });
            }
        }

        // Iterate over every child folder and share them 
        private async Task UnShareFolderSubfolders(Guid folderId, int userId)
        {
            Folder folder = await _context.Folders.GetFolderById(folderId);
            foreach (Folder subfolder in folder.Subfolders)
            {
                await UnShareFolderSubfolders(subfolder.Id, userId);
                await UnShareFolderFiles(subfolder.Id, userId);
            }
            await ShareFolderFiles(folderId, userId);
            if ((await _context.FolderShares.FolderShareExists(folderId, userId)))
                await _context.FolderShares.Delete(folderId, userId);

        }
        // Iterate over every file in the given folder
        private async Task UnShareFolderFiles(Guid folderId, int userId)
        {
            Folder folder = await _context.Folders.GetFolderById(folderId);
            foreach (File file in folder.Files)
            {
                if ((await _context.FileShares.FileShareExists(file.Id, userId)))
                    await _context.FileShares.Delete(file.Id, userId);
            }

        }
    }
}
