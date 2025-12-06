using WebAPi_JWT.WebApi.Core.Domian;

namespace WebAPi_JWT.WebApi.Core.ServiceContract
{
    public interface ITokenRepository
    {
        Task AddRefreshToken(RefreshToken refreshToken);
        Task<bool> IsRefreshTokenValid(string refreshToken);
        Task DisableRefreshToken(int userId);
        Task<int> GetUserIdByRefreshToken(string refreshToken);
    }
}
