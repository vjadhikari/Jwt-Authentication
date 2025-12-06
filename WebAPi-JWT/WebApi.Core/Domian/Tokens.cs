namespace WebAPi_JWT.WebApi.Core.Domian
{
    public class Tokens
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int UserId { get; set; }
    }
}
