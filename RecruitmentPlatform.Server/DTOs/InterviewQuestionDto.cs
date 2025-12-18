namespace RecruitmentPlatform.Server.DTOs
{
    public class InterviewQuestionDto
    {
        public Guid Id { get; set; }
        public string Section { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int TimeLimitSeconds { get; set; } = 60;
    }
}
