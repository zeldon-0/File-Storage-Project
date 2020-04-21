using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Interfaces
{
    public interface ICompositeRepository<T> where T:class
    {
        Task<T> GetByID(Guid itemId, int userId);
        Task<IEnumerable<T>> GetAll();
        Task<T> Create(T t);
        Task Delete(Guid itemId, int userId);
        Task Update(T t);
        Task Save();
    }
}
