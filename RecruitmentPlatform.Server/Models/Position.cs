namespace RecruitmentPlatform.Server.Models
{
    public class Position
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ExperienceLevel Level { get; set; }
        public string TechStack { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum ExperienceLevel
    {
        Junior = 1,
        MidLevel,
        Senior,
        Lead
    }
}
