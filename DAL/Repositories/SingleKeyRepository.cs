using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.Repositories
{
    public class SingleKeyRepository<T, Identificator> : ISingleKeyRepository<T, Identificator>
        where T : class, IEntity<Identificator>
        where Identificator : IComparable
    {
        private FileStorageContext _context;
        public SingleKeyRepository(FileStorageContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T t)
        {
            if (t != null)
                await _context.Set<T>().AddAsync(t);
            await Save();
            return t;
        }

        public async Task Delete(Identificator id)
        {
            T t = await _context.Set<T>().FindAsync(id);

            if (t != null)
                _context.Set<T>().Remove(t);
            await Save();
        }


        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(T t)
        {
            T item = await _context.Set<T>().FindAsync(t.Id);
            if (item != null)
            {
                _context.Entry(item).CurrentValues.SetValues(t);
                _context.Entry(item).State = EntityState.Modified;
                await Save();
            }
        }
    }
}
