using System.ComponentModel.DataAnnotations;

namespace MiniEcom.Api.Dtos
{
    public class UserRegisterDto
    {
        [Required] public string Username { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, MinLength(6)] public string Password { get; set; }
    }
}
