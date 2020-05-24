using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
namespace DAL.Interfaces
{
    public interface IRefreshTokenRepository : ISingleKeyRepository<RefreshToken, int>
    {

        Task<IEnumerable<RefreshToken>> GetRefreshTokensByUser(int userId);
        Task<RefreshToken> FindToken(string refreshToken, int userId);
        Task ClearExpiredTokens(int userId);

    }
}
