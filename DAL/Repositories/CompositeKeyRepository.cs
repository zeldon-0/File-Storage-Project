using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.Repositories
{
    public class CompositeKeyRepository<T> : ICompositeKeyRepository<T>
        where T : class, ICompositeEntity
    {
        private FileStorageContext _context;
        public CompositeKeyRepository(FileStorageContext context)
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

        public async Task Delete(Guid resourceId, int userId)
        {
            T t = await _context.Set<T>().FindAsync(resourceId, userId);

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
            T item = await _context.Set<T>().FindAsync(t.ResourceId, t.UserId);
            if (item != null)
            {
                _context.Entry(item).CurrentValues.SetValues(t);
                _context.Entry(item).State = EntityState.Modified;
                await Save();
            }
        }
    }
}
