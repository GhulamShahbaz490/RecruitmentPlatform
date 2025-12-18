namespace RecruitmentPlatform.Server.Models
{
    public class InterviewSettings
    {
        public Guid Id { get; set; }
        public int PassThresholdPercentage { get; set; } = 70;
        public int QuestionTimeLimitSeconds { get; set; } = 60;
        public int TotalQuestionsPerSection { get; set; } = 5;
        public bool RandomizeQuestions { get; set; } = true;
        public bool EmailNotificationsEnabled { get; set; } = true;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
