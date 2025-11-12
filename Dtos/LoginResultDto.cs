namespace MiniEcom.Dtos
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string UserName { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public string? Token { get; set; }
    }
}
