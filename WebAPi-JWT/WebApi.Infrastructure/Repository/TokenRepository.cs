using Microsoft.EntityFrameworkCore;
using WebAPi_JWT.Infrastructure;
using WebAPi_JWT.WebApi.Core.Domian;
using WebAPi_JWT.WebApi.Core.ServiceContract;

namespace WebAPi_JWT.WebApi.Infrastructure.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly DataAccess _dataAccess;
        public TokenRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _dataAccess.Token.AddAsync(refreshToken);
            await _dataAccess.SaveChangesAsync();
        }

        public async Task DisableRefreshToken(int userId)
        {
            await _dataAccess.Token
                .Where(t => t.UserId == userId && t.Enabled)
                .ForEachAsync(t => t.Enabled = false);

            await _dataAccess.SaveChangesAsync();
        }

        public async Task<int> GetUserIdByRefreshToken(string refreshToken)
        {
            var user= await _dataAccess.Token.FirstAsync(t => t.Token == refreshToken);
            return user.UserId;
        }

        public async Task<bool> IsRefreshTokenValid(string token)
        {
            return await _dataAccess.Token.AnyAsync(t => t.Token == token && t.Enabled && t.ExpiresOn>DateTime.UtcNow);
        }
    }
}
