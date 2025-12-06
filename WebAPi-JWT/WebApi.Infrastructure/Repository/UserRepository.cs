using Microsoft.EntityFrameworkCore;
using WebAPi_JWT.Infrastructure;
using WebAPi_JWT.WebApi.Core.Domian;
using WebAPi_JWT.WebApi.Core.ServiceContract;

namespace WebAPi_JWT.WebApi.Infrastructure.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DataAccess _dataAccess;
        public UserRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public async Task AddUser(User user)
        {
            await _dataAccess.Users.AddAsync(user);
            await _dataAccess.SaveChangesAsync();
        }
        public async Task<int> GetUserIdByUserName(string userName)
        {
            var user = await _dataAccess.Users.FirstOrDefaultAsync(u => u.Email == userName);
            return user != null ? user.Id : 0;
        }
        public async Task<User> GetUserByUserName(string userName)
        {
            return await _dataAccess.Users.FirstOrDefaultAsync(u => u.Email == userName);
        }
        public async Task<User> GetUserById(int userId)
        {
            return await _dataAccess.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }   
    }
}
