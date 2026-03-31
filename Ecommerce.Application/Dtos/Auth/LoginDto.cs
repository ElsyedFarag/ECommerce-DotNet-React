using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Dtos.Auth
{
    public class LoginDto
    {
        [Required, StringLength(128)]
        public string Email { get; set; } = null!;

        [Required, StringLength(256)]
        public string Password { get; set; } = null!;
    }
}