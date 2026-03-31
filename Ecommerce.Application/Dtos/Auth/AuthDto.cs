namespace Ecommerce.Application.Dtos.Auth
{
    public class AuthDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }
    }
}
