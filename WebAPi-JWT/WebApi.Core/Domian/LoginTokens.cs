namespace WebAPi_JWT.WebApi.Core.Domian
{
    public class LoginTokens
    {
        public string ? AccessToken { get; set; }
        public RefreshToken? RefreshToken { get; set; }
    }
}
