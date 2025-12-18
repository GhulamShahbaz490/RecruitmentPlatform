namespace RecruitmentPlatform.Server.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Guid InterviewSessionId { get; set; }
        public InterviewSession InterviewSession { get; set; } = null!;
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int SelectedAnswerIndex { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
        public int TimeTakenSeconds { get; set; }
    }
}
