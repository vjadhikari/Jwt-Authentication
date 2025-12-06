using WebAPi_JWT.WebApi.Core.Domian;

namespace WebAPi_JWT.WebApi.Core.ServiceContract
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<int> GetUserIdByUserName(string userName);
        Task<User> GetUserById(int userId);
    }
}
