namespace RecruitmentPlatform.Server.DTOs
{
    public class RegisterApplicationDto
    {
        public Guid PositionId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public IFormFile Resume { get; set; } = null!;
    }
}
