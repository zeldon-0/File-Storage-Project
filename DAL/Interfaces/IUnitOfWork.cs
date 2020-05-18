using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IFileRepository Files { get; }
        IFolderRepository Folders { get; }
        IUserRepository Users { get; }
        IFileShareRepository FileShares { get; }
        IFolderShareRepository FolderShares { get; }
        IRefreshTokenRepository RefreshTokens { get; }
    }
}
