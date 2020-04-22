using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
namespace DAL.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        ISingularKeyRepository<File, Guid> Files { get; }
        ISingularKeyRepository<Folder, Guid> Folders { get; }
        ISingularKeyRepository<User, int> Users { get; }
        ICompositeKeyRepository<FileShare> FileShares { get; }
        ICompositeKeyRepository<FolderShare> FolderShares { get; }



    }
}
