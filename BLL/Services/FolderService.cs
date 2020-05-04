using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using DAL.Entities;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;

namespace BLL.Services
{
    public class FolderService : IFolderService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public FolderService( IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;

        }
        public async Task<FolderDTO> CopyFolder(Guid folderId)
        {
            Folder folderCopy = await _uow.Folders.GetFolderById(folderId);
            if (folderCopy == null)
                throw new ArgumentException("The requested subfolder does not exist");
            folderCopy.Name = "Copy of " + folderCopy.Name;

            folderCopy = await _uow.Folders.Create(folderCopy);

            return _mapper.Map<FolderDTO>(folderCopy);

        }

        public async Task<FolderDTO> CreateAtFolder(FolderDTO folder, Guid folderId)
        {
            folder.ParentId= folderId;
            Folder createdFolder = await _uow.Folders.Create(_mapper.Map<Folder>(folder));
            return _mapper.Map<FolderDTO>(createdFolder);
        }

        public async Task<FolderDTO> CreateAtRoot(FolderDTO folder)
        { 
            Folder createdFolder = await _uow.Folders.Create(_mapper.Map<Folder>(folder));
            return _mapper.Map<FolderDTO>(createdFolder);
        }

        public async Task Delete(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new ArgumentNullException("The provided folder does not exist");

            foreach (var file in folder.Files)
            {
                await _uow.Files.Delete(file.Id);
            }

            foreach ( var subfolder in folder.Subfolders)
            {
                await Delete(subfolder.Id);
            }
            await _uow.Folders.Delete(folderId);
        }

        public async Task<IEnumerable<FolderDTO>> GetFolderSubfolders(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            if (folder == null)
                throw new ArgumentNullException("The provided folder does not exist");
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
