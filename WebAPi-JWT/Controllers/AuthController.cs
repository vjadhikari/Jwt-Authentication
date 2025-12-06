using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPi_JWT.Infrastructure;
using WebAPi_JWT.WebApi.Core.Domian;
using WebAPi_JWT.WebApi.Core.ServiceContract;
using WebAPi_JWT.WebApi.Core.Services;

namespace WebAPi_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private ITokenProvider _tokenProvider;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(ITokenProvider tokenProvider, IAuthRepository authRepository, IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            _tokenProvider = tokenProvider;
            _authRepository = authRepository;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Enter Valid UserName and Password");
            if (await _authRepository.CheckUserExists(userName))
                return BadRequest("User Already Exists");

            var registerUser = new AuthUser();
            registerUser.Username = userName;
            registerUser.Password = new PasswordHasher<AuthUser>().HashPassword(registerUser, password);

            await _userRepository.AddUser(new User { Email = userName });

            registerUser.UserId = await _userRepository.GetUserIdByUserName(userName);
            await _authRepository.AddAuthUser(registerUser);

            LoginTokens token = _tokenProvider.GenerateToken(userName);
            await _tokenRepository.AddRefreshToken(new RefreshToken
            {
                CreatedOn = token.RefreshToken.CreatedOn,
                ExpiresOn = token.RefreshToken.ExpiresOn,
                Enabled = token.RefreshToken.Enabled,
                Token = token.RefreshToken.Token,
                UserId = registerUser.UserId,
            });
            return Ok(new Tokens { AccessToken=token.AccessToken,RefreshToken=token.RefreshToken.Token});
        }
        [HttpGet("Login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return Unauthorized("Enter Correct UserName and Password");
            if (! await _authRepository.ValidateUserCred(new AuthUser { Username = userName, Password = password }))
                return Unauthorized("Enter Correct UserName and Password");

            int userId = await _userRepository.GetUserIdByUserName(userName);

            await _tokenRepository.DisableRefreshToken(userId);

            LoginTokens token = _tokenProvider.GenerateToken(userName);
            await _tokenRepository.AddRefreshToken(new RefreshToken
            {
                CreatedOn = token.RefreshToken.CreatedOn,
                ExpiresOn = token.RefreshToken.ExpiresOn,
                Enabled = token.RefreshToken.Enabled,
                Token = token.RefreshToken.Token,
                UserId = userId,
            });

            return Ok(new Tokens { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken.Token });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            string? refreshToken = Request.Cookies["RefreshToken"].ToString();
            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("Invalid Refresh Token");
            if (! await _tokenRepository.IsRefreshTokenValid(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            int userId = await _tokenRepository.GetUserIdByRefreshToken(refreshToken);
            User user= await _userRepository.GetUserById(userId);
            await _tokenRepository.DisableRefreshToken(userId);
            LoginTokens token =  _tokenProvider.GenerateToken(user.Email);
            RefreshToken? newRefreshToken = token.RefreshToken;
            newRefreshToken.UserId = userId;
            await _tokenRepository.AddRefreshToken(newRefreshToken);
            return Ok(new Tokens { AccessToken = token.AccessToken, RefreshToken = token?.RefreshToken?.Token });
        }
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            string? refreshToken = Request.Cookies["RefreshToken"].ToString();
            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized("Invalid Refresh Token");
            if (!await _tokenRepository.IsRefreshTokenValid(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            int userId = await _tokenRepository.GetUserIdByRefreshToken(refreshToken);
            await _tokenRepository.DisableRefreshToken(userId);
            return Ok("Logged Out Successfully");
        }
    }
}
