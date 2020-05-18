using System;
using System.Collections.Generic;
using DAL.Interfaces;
using DAL.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using System.Linq;
namespace DAL.Repositories
{
    public class RefreshTokenRepository : SingleKeyRepository<RefreshToken, int>, IRefreshTokenRepository
    {
        private FileStorageContext _context;
        public RefreshTokenRepository(FileStorageContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RefreshToken>> GetRefreshTokensByUser(int userId)
        {
            IEnumerable<RefreshToken> refreshTokens =
                await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync();
            return refreshTokens;
        }

        public async Task<RefreshToken> FindToken(string refreshToken, int userId)
        {
            IEnumerable<RefreshToken> refreshTokens =
                await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync();
            RefreshToken token = refreshTokens
                .FirstOrDefault(rt => rt.Token == refreshToken );
            return token;
        }
    }
}
