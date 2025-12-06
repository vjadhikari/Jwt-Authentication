using WebAPi_JWT.WebApi.Core.Domian;

namespace WebAPi_JWT.WebApi.Core.ServiceContract
{
    public interface IAuthRepository
    {
        Task AddAuthUser(AuthUser authUser);
        Task<bool> CheckUserExists(string userName);
        Task<AuthUser> GetAuthUser(string userName);
        Task<bool> ValidateUserCred(AuthUser loginUser);
    }
}
