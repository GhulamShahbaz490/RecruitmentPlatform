namespace RecruitmentPlatform.Server.Models
{
    public class Result
    {
        public Guid Id { get; set; }
        public Guid InterviewSessionId { get; set; }
        public InterviewSession InterviewSession { get; set; } = null!;
        public int TotalScore { get; set; }
        public int MaxPossibleScore { get; set; }
        public double Percentage { get; set; }
        public ResultStatus Status { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public enum ResultStatus
    {
        Pass,
        Fail,
        PendingPhysicalInterview
    }
}
