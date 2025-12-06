using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPi_JWT.Infrastructure;
using WebAPi_JWT.WebApi.Core.Domian;
using WebAPi_JWT.WebApi.Core.ServiceContract;

namespace WebAPi_JWT.WebApi.Infrastructure.Repository
{
    public class AuthRepository: IAuthRepository
    {
        private readonly DataAccess _dataAccess;
        public AuthRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public async Task AddAuthUser(AuthUser authUser)
        {
            await _dataAccess.AuthUsers.AddAsync(authUser);
            await _dataAccess.SaveChangesAsync();
        }
        public async Task<bool> CheckUserExists(string userName)
        {
            return await _dataAccess.AuthUsers.AnyAsync(x=>x.Username==userName);
        }
        public async Task<AuthUser> GetAuthUser(string userName)
        {
            return await _dataAccess.AuthUsers.FirstOrDefaultAsync(u => u.Username == userName);
        }
        public async Task<bool> ValidateUserCred(AuthUser loginUser)
        {
            if (loginUser == null)
            {
                return false;
            }
            AuthUser userDetail = await GetAuthUser(loginUser.Username);
            if (userDetail == null) 
            {
                return false;
            }
            if (new PasswordHasher<AuthUser>().VerifyHashedPassword(loginUser, userDetail.Password, loginUser.Password) == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
            return true;
        }
    }
}
