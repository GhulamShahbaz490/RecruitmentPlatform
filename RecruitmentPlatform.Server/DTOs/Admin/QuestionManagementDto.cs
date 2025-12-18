namespace RecruitmentPlatform.Server.DTOs.Admin
{
    public class QuestionManagementDto
    {
        public Guid Id { get; set; }
        public string Section { get; set; } = string.Empty; // "Basic", "Technical", "Practical"
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
        public int Points { get; set; } = 10;
        public string Explanation { get; set; } = string.Empty;
    }

    public class CreateQuestionDto
    {
        public string Section { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
        public int Points { get; set; } = 10;
        public string Explanation { get; set; } = string.Empty;
    }

    public class UpdateQuestionDto : CreateQuestionDto
    {
        public Guid Id { get; set; }
    }
}
