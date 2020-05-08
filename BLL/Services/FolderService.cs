using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using BLL.Models;
using BLL.Exceptions;
using DAL.Interfaces;
using DAL.Entities;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlTypes;

namespace BLL.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;


        public FolderService( IUnitOfWork uow,  IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;

        }
        private async Task CloneSubfolders(Guid subfolderId, Guid parentId)
        {
            FolderDTO subfolder= await GetFolderById(subfolderId);
            subfolder.Id = new Guid();
            subfolder.Owner = null;

            Folder newFolder = new Folder()
            {
                Name = subfolder.Name,
                Description = subfolder.Description,
                OwnerId = subfolder.OwnerId,
                ParentId = parentId
            };
            newFolder = await _uow.Folders.Create(newFolder);


            if (subfolder.Subfolders != null && subfolder.Subfolders.Any())
             {
                foreach (var folder in subfolder.Subfolders)
                {
                    await CloneSubfolders(folder.Id, newFolder.Id);
                }
             }

            if (subfolder.Files != null && subfolder.Files.Any())
            {
                foreach (var file in subfolder.Files)
                {
                    await _uow.Files.Create(
                        new File()
                        {
                            Name = file.Name,
                            OwnerId = file.OwnerId,
                            Description = file.Description,
                            URL = file.URL,
                            FolderId = newFolder.Id
                        }
                    );
                }
            }
        }
        public async Task<FolderDTO> CopyFolder(Guid folderId)
        {
            Folder folderCopy = await _uow.Folders.GetFolderById(folderId);
            if (folderCopy == null)
                throw new NotFoundException("The requested subfolder does not exist");

            var newFolder = new Folder()
            {
                Name = "Copy of " + folderCopy.Name,
                Description = folderCopy.Description,
                OwnerId = folderCopy.OwnerId,
                ParentId = folderCopy.ParentId,
                ShareStatus = folderCopy.ShareStatus
            };
            newFolder = await _uow.Folders.Create(newFolder);

            if (folderCopy.Files != null && folderCopy.Files.Any())
            {
                foreach (var file in folderCopy.Files)
                {
                    await _uow.Files.Create(
                        new File()
                        {
                            Name = file.Name,
                            OwnerId = file.OwnerId,
                            Description = file.Description,
                            URL = file.URL,
                            FolderId = newFolder.Id
                        }
                    );
                }
            }

            foreach (var subfolder in folderCopy.Subfolders)
                await CloneSubfolders(subfolder.Id, newFolder.Id);

            return _mapper.Map<FolderDTO>(newFolder);

        }

        public async Task<FolderDTO> CreateAtFolder(FolderDTO folder, Guid folderId, int userId)
        {
            Folder parent = await _uow.Folders.GetFolderById(folderId);
           
            if (parent == null)
                throw new NotFoundException("The parent folder does not exist");
             
            folder.ParentId= folderId;
            folder.OwnerId = userId;
            Folder createdFolder = await _uow.Folders.Create(_mapper.Map<Folder>(folder));
            return _mapper.Map<FolderDTO>(createdFolder);
        }

        public async Task<FolderDTO> CreateAtRoot(FolderDTO folder, int userId)
        {
            folder.OwnerId = userId;
            Folder createdFolder = await _uow.Folders.Create(_mapper.Map<Folder>(folder));
            return _mapper.Map<FolderDTO>(createdFolder);
        }

        public async Task<FolderDTO> GetFolderById(Guid folderId)
        {
            Folder folder =  await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new NotFoundException("The requested folder does not exist");
            return _mapper.Map<FolderDTO>(folder);
        }

        public async Task Delete(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new NotFoundException("The provided folder does not exist");

            if (folder.Files != null && folder.Files.Any())
            {
                foreach (var file in folder.Files)
                {
                    await _uow.Files.Delete(file.Id);
                }
            }
            

            if (folder.Subfolders != null && folder.Subfolders.Any())
            {
                foreach (var subfolder in folder.Subfolders)
                {
                    await Delete(subfolder.Id);
                }
            }
            await _uow.Folders.Delete(folderId);
        }

        public async Task<IEnumerable<FolderDTO>> GetFolderSubfolders(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new NotFoundException("The requested folder does not exist");

            return folder.Subfolders.Select(sf => _mapper.Map<FolderDTO>(sf));
        }

        public async Task<IEnumerable<FolderDTO>> GetUserFolders(int userId)
        {
            User user = await _uow.Users.GetUserById(userId);
            if (user == null)
                throw new NotFoundException("The requested user is not registered.");
            IEnumerable<Folder> folders = await _uow.Folders.GetUserFolders(userId);
            return folders.Where(f => f.Parent == null)
                .Select(f => _mapper.Map<FolderDTO>(f));
        }

        public async Task MoveToFolder(Guid subfolderId, Guid parentFolderId)
        {
            Folder folderToMove = await _uow.Folders.GetFolderById(subfolderId);
            if (folderToMove == null)
                throw new NotFoundException("The requested folder does not exist");
            Folder targetFolder = await _uow.Folders.GetFolderById(parentFolderId);
            if (targetFolder == null)
                throw new NotFoundException("The target folder does not exist");
            folderToMove.ParentId = parentFolderId;
            await _uow.Folders.Update(folderToMove);

        }

        public async Task Update(FolderDTO folder)
        {
            if (folder == null)
                throw new BadRequestException("Provided folder model is null.");
            await _uow.Folders.Update(_mapper.Map<Folder>(folder));
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
        ~FolderService()
        {
            Dispose(false);
        }
    }
}
