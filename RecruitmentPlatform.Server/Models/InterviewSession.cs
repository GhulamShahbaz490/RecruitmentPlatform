namespace RecruitmentPlatform.Server.Models
{
    public class InterviewSession
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Application Application { get; set; } = null!;
        public InterviewStatus Status { get; set; } = InterviewStatus.NotStarted;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TotalScore { get; set; }
        public int MaxPossibleScore { get; set; }
        public Result Result { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

    public enum InterviewStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Expired
    }
}
