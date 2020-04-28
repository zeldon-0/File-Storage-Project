using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
namespace BLL.Interfaces
{
    interface IShareStatusService : IDisposable
    {
        Task MakeFileShareable(Guid fileId);
        Task MakeFolderShareable(Guid folderId);
        Task MakeFileUnshareable(Guid fileId);
        Task MakeFolderUnshareable(Guid folderId);
    }
}
