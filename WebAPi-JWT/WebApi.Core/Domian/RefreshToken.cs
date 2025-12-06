using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPi_JWT.WebApi.Core.Domian
{
    public class RefreshToken
    {
        public int id { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Enabled { get; set; }
        public int UserId { get; set; }
    }
}
