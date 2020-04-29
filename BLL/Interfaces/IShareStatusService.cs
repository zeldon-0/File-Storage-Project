using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
namespace BLL.Interfaces
{
    interface IShareStatusService : IDisposable
    {
        Task MakeFileShareable(FileDTO file);
        Task MakeFolderShareable(FolderDTO folder);
        Task MakeFileUnshareable(FileDTO file);
        Task MakeFolderUnshareable(FolderDTO folder);
    }
}
