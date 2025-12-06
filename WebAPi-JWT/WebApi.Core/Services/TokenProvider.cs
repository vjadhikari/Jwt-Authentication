using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebAPi_JWT.WebApi.Core.Domian;

namespace WebAPi_JWT.WebApi.Core.Services
{
    public interface ITokenProvider
    {
        LoginTokens GenerateToken(string userName);
    }
    public class TokenProvider: ITokenProvider
    {
        public IConfiguration _configuration { get; }
        public TokenProvider(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }
        public LoginTokens GenerateToken(string userName)
        {
            LoginTokens tokens = new LoginTokens();
            tokens.AccessToken = GenerateAccessToken(userName);
            tokens.RefreshToken = GenerateRefreshToken();
            return tokens;
        }
        public string GenerateAccessToken(string userName)
        {
            string? secretKey = _configuration["Jwt:Key"];
            if(string.IsNullOrWhiteSpace(secretKey))
            {
                throw new Exception("JWT Secret Key is not configured.");
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, userName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = creds
            };
            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                ExpiresOn = DateTime.UtcNow.AddHours(7),
                CreatedOn = DateTime.UtcNow,
                Enabled = true
            };
        }
    }
}
