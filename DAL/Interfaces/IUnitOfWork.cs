using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
namespace DAL.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        ISingularRepository<File, Guid> Files { get; }
        ISingularRepository<Folder, Guid> Folders { get; }
        ISingularRepository<User, int> Users { get; }
        ICompositeRepository<FileShare> FileShares { get; }
        ICompositeRepository<FolderShare> FolderShares { get; }



    }
}
