namespace MiniEcom.Dtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
