using System;
using System.Threading.Tasks;
using DAL.Entities;
namespace DAL.Interfaces
{
    public interface IFileShareRepository : ICompositeKeyRepository<FileShare>
    {
        Task<bool> FileShareExists(Guid fileId, int userId);
    }
}
