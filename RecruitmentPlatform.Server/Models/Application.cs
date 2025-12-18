namespace RecruitmentPlatform.Server.Models
{
    public class Application
    {
        public Guid Id { get; set; }
        public Guid PositionId { get; set; }
        public Position Position { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string InterviewNumber { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public ICollection<InterviewSession> InterviewSessions { get; set; } = new List<InterviewSession>();
    }
}
