﻿using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using BLL.Models;
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
        private IUnitOfWork _uow;
        private IMapper _mapper;


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
            //FolderDTO newFolder = await CreateAtFolder(subfolder, parentId, subfolder.OwnerId);
            Folder newFolder = new Folder()
            {
                Name = subfolder.Name,
                Description = subfolder.Description,
                OwnerId = subfolder.OwnerId,
                ParentId = subfolder.ParentId
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
                throw new ArgumentException("The requested subfolder does not exist");
            folderCopy.Name = "Copy of " + folderCopy.Name;
            folderCopy.Owner = null;
            var newFolder = new Folder()
            {
                Name = "Copy of " + folderCopy.Name,
                Description = folderCopy.Description,
                OwnerId = folderCopy.OwnerId,
                ParentId = folderCopy.ParentId,
                ShareStatus = folderCopy.ShareStatus
            };
            newFolder = await _uow.Folders.Create(newFolder);
            foreach (var subfolder in folderCopy.Subfolders)
                await CloneSubfolders(subfolder.Id, newFolder.Id);

            return _mapper.Map<FolderDTO>(folderCopy);

        }

        public async Task<FolderDTO> CreateAtFolder(FolderDTO folder, Guid folderId, int userId)
        {
            Folder parent = await _uow.Folders.GetFolderById(folderId);
           
            if (parent == null)
                throw new ArgumentException("The parent folder does not exist");
             
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
            return _mapper.Map<FolderDTO>(folder);
        }

        public async Task Delete(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new ArgumentNullException("The provided folder does not exist");

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
                throw new ArgumentNullException("The requested folder does not exist");
            folder.Subfolders = null;
            return folder.Subfolders.Select(sf => _mapper.Map<FolderDTO>(sf));
        }

        public async Task<IEnumerable<FolderDTO>> GetUserFolders(int userId)
        {
            IEnumerable<Folder> folders = await _uow.Folders.GetUserFolders(userId);
            return folders.Where(f => f.Parent == null)
                .Select(f => _mapper.Map<FolderDTO>(f));
        }

        public async Task MoveToFolder(Guid subfolderId, Guid parentFolderId)
        {
            Folder folderToMove = await _uow.Folders.GetFolderById(subfolderId);
            if (folderToMove == null)
                throw new ArgumentException("The requested folder does not exist");
            Folder targetFolder = await _uow.Folders.GetFolderById(parentFolderId);
            if (targetFolder == null)
                throw new ArgumentException("The target folder does not exist");
            folderToMove.ParentId = parentFolderId;
            await _uow.Folders.Update(folderToMove);

        }

        public async Task Update(FolderDTO folder)
        {
            if (folder == null)
                throw new ArgumentNullException("Provided folder model is null.");
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
