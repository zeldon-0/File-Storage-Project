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

namespace BLL.Services
{
    public class FileService : IFileService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public FileService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task MoveToFolder(Guid fileId, Guid folderId)
        {
            File fileToMove = await _uow.Files.GetFileById(fileId);
            if (fileToMove == null)
                throw new ArgumentException("The requested file does not exist");
            Folder targetFolder = await _uow.Folders.GetFolderById(folderId);
            if (targetFolder == null)
                throw new ArgumentException("The requested folder does not exist");
            fileToMove.FolderId = folderId;
            await _uow.Files.Update(fileToMove);

        }

        public async Task<FileDTO> CreateAtFolder(FileDTO file, Guid folderId)
        {
            file.FolderId = folderId;
            File createdFile = await _uow.Files.Create(_mapper.Map<File>(file));
            return _mapper.Map<FileDTO>(createdFile);
        }

        public async Task<FileDTO> CreateAtRoot(FileDTO file)
        {
            File createdFile = await _uow.Files.Create(_mapper.Map<File>(file));
            return _mapper.Map<FileDTO>(createdFile);
        }

        public async Task Delete(Guid fileId)
        {
            File file = await _uow.Files.GetFileById(fileId);
            if (file == null)
                throw new ArgumentException("The requested file does not exist");
            await _uow.Files.Delete(fileId);

        }


        public async Task<IEnumerable<FileDTO>> GetFolderFiles(Guid folderId)
        {
            Folder folder = await _uow.Folders.GetFolderById(folderId);
            return folder.Files.Select(f => _mapper.Map<FileDTO>(f));
        }

        public async Task<IEnumerable<FileDTO>> GetUserFiles(int userId)
        {
            IEnumerable<File> files = await _uow.Files.GetUserFiles(userId);
            return files.Where(f => f.Folder == null)
                .Select(f => _mapper.Map<FileDTO>(f));
        }

        public async Task Update(FileDTO file)
        {
            if (file == null)
                throw new ArgumentNullException("Provided file is null.");
            await _uow.Files.Update(_mapper.Map<File>(file));
        }

        public async Task<FileDTO> CopyFile(Guid fileId)
        {
            File fileCopy = await _uow.Files.GetFileById(fileId);
            if (fileCopy == null)
                throw new ArgumentException("The requested file does not exist");
            fileCopy.Name = "Copy of " + fileCopy.Name;

            fileCopy = await _uow.Files.Create(fileCopy);

            return _mapper.Map<FileDTO>(fileCopy);

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
        ~FileService()
        {
            Dispose(false);
        }

    }
}