using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    public class UserRepository : ISingularRepository<User, int>
    {
        private FileStorageContext _context;
        public UserRepository(FileStorageContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User u)
        {
            if (u != null)
                await _context.Users.AddAsync(u);
            await Save();
            return u;
        }

        public async Task Delete(int id)
        {
            User u = await _context.Users.FindAsync(id);

            if (u != null)
                _context.Users.Remove(u);
            await Save();

        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByID(int id)
        {

            return await _context.Users
                   .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(User u)
        {
            if (u == null)
                return;

            User user = await GetByID(u.Id);
            if (user != null)
            {
                _context.Entry(user).CurrentValues.SetValues(u);
                _context.Entry(user).State = EntityState.Modified;
            }
            await Save();

        }
    }
}
