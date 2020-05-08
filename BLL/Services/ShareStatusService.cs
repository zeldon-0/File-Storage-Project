using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using DAL.Entities;
using AutoMapper;

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
            file.ShareStatus = ShareStatusDTO.Shareable;
            await _uow.Files.Update(_mapper.Map<File>(file));
        }

        public async Task MakeFileUnshareable(FileDTO file)
        {
            file.ShareStatus = ShareStatusDTO.Private;
            await _uow.Files.Update(_mapper.Map<File>(file));
        }

        public async Task MakeFolderShareable(FolderDTO folder)
        {
            folder.ShareStatus = ShareStatusDTO.Shareable;
            await _uow.Folders.Update(_mapper.Map<Folder>(folder));
        }

        public async Task MakeFolderUnshareable(FolderDTO folder)
        {
            folder.ShareStatus = ShareStatusDTO.Private;
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
        ~ShareStatusService()
        {
            Dispose(false);
        }
    }
}
