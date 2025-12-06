using System.ComponentModel.DataAnnotations;

namespace WebAPi_JWT.WebApi.Core.Domian
{
    public class AuthUser
    {
        public int id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
