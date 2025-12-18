namespace RecruitmentPlatform.Server.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public QuestionSection Section { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
        public int Points { get; set; } = 10;
        public string Explanation { get; set; } = string.Empty;
    }

    public enum QuestionSection
    {
        Basic,
        Technical,
        Practical
    }
}
