using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using DAL.Entities;
using AutoMapper;
using BLL.Exceptions;
namespace BLL.Services
{
    public class ShareStatusService : IShareStatusService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ShareStatusService(IUnitOfWork uow, IMapper mapper)
        {
            _uow= uow;
            _mapper = mapper;
        }

        public async Task MakeFileShareable(FileDTO file)
        {
            File fileInDB = await _uow.Files.GetFileById(file.Id);
            if (fileInDB == null)
                throw new NotFoundException("The file corresponding to the model does not exist");
            file.ShareStatus = ShareStatusDTO.Shareable;
            file.LastChange = DateTime.Now;
            await _uow.Files.Update(_mapper.Map<File>(file));
        }

        public async Task MakeFileUnshareable(FileDTO file)
        {
            File fileInDB = await _uow.Files.GetFileById(file.Id);
            if (fileInDB == null)
                throw new NotFoundException("The file corresponding to the model does not exist");

            file.ShareStatus = ShareStatusDTO.Private;
            file.LastChange = DateTime.Now;
            await _uow.Files.Update(_mapper.Map<File>(file));
        }

        public async Task MakeFolderShareable(FolderDTO folder)
        {
            Folder folderInDB = await _uow.Folders.GetFolderById(folder.Id);
            if (folderInDB == null)
                throw new NotFoundException("The folder corresponding to the model does not exist");

            await ShareFolderSubfolders(folder.Id);
        }

        public async Task MakeFolderUnshareable(FolderDTO folder)
        {
            Folder folderInDB = await _uow.Folders.GetFolderById(folder.Id);
            if (folderInDB == null)
                throw new NotFoundException("The folder corresponding to the model does not exist");

            await UnShareFolderSubfolders(folder.Id);
        }

        // Iterate over every child folder and share them 
        private async Task ShareFolderSubfolders(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Subfolders != null)
            {
                foreach (Folder subfolder in folder.Subfolders)
                {
                    await ShareFolderSubfolders(subfolder.Id);
                    await ShareFolderFiles(subfolder.Id);
                }
            }
            await ShareFolderFiles(folderId);
            if (folder.ShareStatus == ShareStatus.Private)
            {
                folder.ShareStatus = ShareStatus.Shareable;
                folder.LastChange = DateTime.Now;
                await _uow.Folders.Update(folder);
            }
        }
        // Iterate over every file in the given folder
        private async Task ShareFolderFiles(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Subfolders != null)
            {
                foreach (File file in folder.Files)
                {
                    if (file.ShareStatus == ShareStatus.Private)
                    {
                        file.ShareStatus = ShareStatus.Shareable;
                        file.LastChange = DateTime.Now;
                        await _uow.Files.Update(file);
                    }
                }
            }
        }

        // Iterate over every child folder and share them 
        private async Task UnShareFolderSubfolders(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Subfolders != null)
            {
                foreach (Folder subfolder in folder.Subfolders)
                {
                    await UnShareFolderSubfolders(subfolder.Id);
                    await UnShareFolderFiles(subfolder.Id);
                }
            }
            await UnShareFolderFiles(folderId);
            if (folder.ShareStatus == ShareStatus.Shareable)
            {
                folder.ShareStatus = ShareStatus.Private;
                folder.LastChange = DateTime.Now;
                await _uow.Folders.Update(folder);
            }

        }
        // Iterate over every file in the given folder
        private async Task UnShareFolderFiles(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder.Files != null)
            { 
                foreach (File file in folder.Files)
                {
                    if (file.ShareStatus == ShareStatus.Shareable)
                    {
                        file.ShareStatus = ShareStatus.Private;
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
        ~ShareStatusService()
        {
            Dispose(false);
        }
    }
}
