namespace RecruitmentPlatform.Server.DTOs
{
    public class SubmitAnswerDto
    {
        public Guid QuestionId { get; set; }
        public int SelectedAnswerIndex { get; set; }
        public int TimeTakenSeconds { get; set; }
    }
}
