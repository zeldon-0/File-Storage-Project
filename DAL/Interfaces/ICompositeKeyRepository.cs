using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Interfaces
{
    public interface ICompositeKeyRepository<T> 
        where T:class, ICompositeEntity
    {

        Task<T> Create(T t);
        Task Delete(Guid itemId, int userId);
        Task Update(T t);
        Task Save();
    }
}
