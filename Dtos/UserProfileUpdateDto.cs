using System.ComponentModel.DataAnnotations;

namespace MiniEcom.Dtos
{
    public class UserProfileUpdateDto
    {
        [MaxLength(100)]
        public string? DisplayName { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
