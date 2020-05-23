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
using BLL.Exceptions;
namespace BLL.Services
{
    public class SharingService : ISharingService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SharingService(UserManager<User> userManager, 
               IUnitOfWork uow, IMapper mapper)
        {
            _userManager = userManager;
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<IEnumerable<FileDTO>> GetSharedFiles(int userId)
        {
            User user = await _uow.Users.GetUserById(userId);
            if (user == null)
                throw new NotFoundException("The requested user is not registered.");
            IEnumerable<File> files = await _uow.Files.GetSharedFiles(userId);
            files = files.Where( (f) => f.FolderId == null
                || !_uow.FolderShares.FolderShareExists(f.FolderId ?? default, user.Id).Result);
            return files.Select(f => _mapper.Map<FileDTO>(f));
        }

        public async Task<IEnumerable<UserDTO>> GetSharedFileUserList(Guid fileId)
        {
            File file = await _uow.Files.GetFileById(fileId);
            if (file == null)
                throw new NotFoundException("The requested file does not exist.");

            IEnumerable<User> users =  await _uow.Users.GetUsersByFileShare(fileId);
            return users.Select(u => _mapper.Map<UserDTO>(u));
        }

        public async Task<IEnumerable<FolderDTO>> GetSharedFolders(int userId)
        {
            User user = await _uow.Users.GetUserById(userId);
            if (user == null)
                throw new NotFoundException("The requested user is not registered.");

            IEnumerable<Folder> folders = await _uow.Folders.GetSharedFolders(userId);
            IEnumerable<Folder> foldersCopy = new List<Folder>();
            
            foreach (var folder in folders)
                foldersCopy.Append(folder);

            folders = folders.Where((f) => f.ParentId == null
               || !_uow.FolderShares.FolderShareExists(f.ParentId ?? default, user.Id).Result);

     
            return folders.Select(f => _mapper.Map<FolderDTO>(f));
        }

        public async Task<IEnumerable<UserDTO>> GetSharedFolderUserList(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new NotFoundException("The requested folder does not exist.");

            IEnumerable<User> users = await _uow.Users.GetUsersByFolderShare(folderId);
            return users.Select(u => _mapper.Map<UserDTO>(u));
        }


        public async Task ShareFile(Guid fileId, string userName)
        {
            File file = await _uow.Files.GetFileById(fileId);
            if (file == null)
                throw new NotFoundException("The file with the provided Id does not exist.");
            
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException("The user with the provided user name is not registered.");

            if (await _uow.FileShares.FileShareExists(fileId, user.Id))
                throw new BadRequestException("The file is already shared with the user");

            await _uow.FileShares.Create(
                new FileShare() { FileId = fileId, UserId = user.Id });

            file.LastChange = DateTime.Now; 
            await _uow.Files.Update(file);
        }


        public async Task ShareFolder(Guid folderId, string userName)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new NotFoundException("The file with the provided Id does not exist");
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException("The user with the provided user name does not exist");
            if (await _uow.FolderShares.FolderShareExists(folderId, user.Id))
                throw new BadRequestException("The folder is already shared with the user");

            await ShareFolderSubfolders(folderId, user.Id);

        }


        public async Task UnshareFile(Guid fileId, string userName)
        {
            File file= await _uow.Files.GetFileById(fileId);
            if (file == null)
                throw new NotFoundException("The file with the provided Id does not exist");

            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException("The user with the provided user name is not yet registered.");

            bool fileShareExists = await _uow.FileShares.FileShareExists(fileId, user.Id);
            if (!fileShareExists)
                throw new NotFoundException("The file is not shared with the provided user");
            else
            {
                await _uow.FileShares.Delete(fileId, user.Id);
                
                file.LastChange = DateTime.Now;
                await _uow.Files.Update(file);
            }
        }


        public async Task UnshareFolder(Guid folderId, string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException("The user with the provided user name is not yet registered.");
            Folder folder = await _uow.Folders.GetFolderById(folderId);

            if (folder == null)
                throw new NotFoundException("The file with the provided Id does not exist");
            
            bool folderShareExists = await _uow.FolderShares.FolderShareExists(folderId, user.Id);
            if (!folderShareExists)
                throw new BadRequestException("The folder is not shared with the provided user");
            else
            {
                await UnShareFolderSubfolders(folderId, user.Id); 
            }
        }

        // Iterate over every child folder and share them 
        private async Task ShareFolderSubfolders(Guid folderId, int userId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Subfolders != null)
            {
                foreach (Folder subfolder in folder.Subfolders)
                {
                    await ShareFolderSubfolders(subfolder.Id, userId);
                    await ShareFolderFiles(subfolder.Id, userId);
                }
            }
            await ShareFolderFiles(folderId, userId);
            if (!(await _uow.FolderShares.FolderShareExists(folderId, userId)))
            {
                await _uow.FolderShares.Create(
                    new FolderShare() { FolderId = folderId, UserId = userId });
                
                folder.LastChange = DateTime.Now;
                await _uow.Folders.Update(folder);
            }

        }
        // Iterate over every file in the given folder
        private async Task ShareFolderFiles(Guid folderId, int userId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Files != null)
            {
                foreach (File file in folder.Files)
                {
                    if (!(await _uow.FileShares.FileShareExists(file.Id, userId)))
                    {
                        await _uow.FileShares.Create(
                            new FileShare() { FileId = file.Id, UserId = userId });
                            
                            file.LastChange = DateTime.Now;
                            await _uow.Files.Update(file);
                    }
                }
            }
        }

        // Iterate over every child folder and share them 
        private async Task UnShareFolderSubfolders(Guid folderId, int userId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Subfolders != null)
            {
                foreach (Folder subfolder in folder.Subfolders)
                {
                    await UnShareFolderSubfolders(subfolder.Id, userId);
                    await UnShareFolderFiles(subfolder.Id, userId);
                }
            }
            await UnShareFolderFiles(folderId, userId);
            if (await _uow.FolderShares.FolderShareExists(folderId, userId))
            {
                await _uow.FolderShares.Delete(folderId, userId);
                
                folder.LastChange = DateTime.Now;
                await _uow.Folders.Update(folder);
            }

        }
        // Iterate over every file in the given folder
        private async Task UnShareFolderFiles(Guid folderId, int userId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Files != null)
            {
                foreach (File file in folder.Files)
                {
                    if (await _uow.FileShares.FileShareExists(file.Id, userId))
                    {
                        await _uow.FileShares.Delete(file.Id, userId);
                        
                        file.LastChange = DateTime.Now;
                        await _uow.Files.Update(file);
                    }
                }
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
        ~SharingService()
        {
            Dispose(false);
        }
    }
}
