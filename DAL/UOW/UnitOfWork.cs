using System;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Context;
using DAL.Repositories;
namespace DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private FileStorageContext _context;
        public UnitOfWork(FileStorageContext context)
        {
            _context = context;
        }

        private FileRepository _fileRepository;
        public IFileRepository Files
        {
            get
            {
                if (_fileRepository == null)
                    _fileRepository = new FileRepository(_context);
                return _fileRepository;
            }
        }


        private FolderRepository _folderRepository;
        public IFolderRepository Folders
        {
            get
            {
                if (_folderRepository == null)
                    _folderRepository = new FolderRepository(_context);
                return _folderRepository;
            }
        }


        private UserRepository _userRepository;
        public IUserRepository Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }


        private FileShareRepository _fileShareRepository;
        public ICompositeKeyRepository<FileShare> FileShares
        {
            get
            {
                if (_fileShareRepository == null)
                    _fileShareRepository = new FileShareRepository(_context);
                return _fileShareRepository;
            }
        }

        private FolderShareRepository _folderShareRepository;
        public ICompositeKeyRepository<FolderShare> FolderShares
        {
            get
            {
                if (_folderShareRepository == null)
                    _folderShareRepository = new FolderShareRepository(_context);
                return _folderShareRepository;
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
                        _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        ~UnitOfWork()
        {
            Dispose(false);
        }

    }
}
