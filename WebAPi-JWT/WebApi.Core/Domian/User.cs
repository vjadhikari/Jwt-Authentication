namespace WebAPi_JWT.WebApi.Core.Domian
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateOnly Dob { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Number { get; set; }
    }
}
