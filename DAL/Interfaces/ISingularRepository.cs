using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Interfaces
{
    interface ISingularRepository<T, Identificator> 
                where T:class 
                where Identificator:IComparable
    {
        Task<T> GetByID(Identificator id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Create(T t);
        Task Delete(Identificator id);
        Task Update(T t);
        Task Save();
    }
}
