namespace RecruitmentPlatform.Server.DTOs.Admin
{
    public class SettingsDto
    {
        public int PassThresholdPercentage { get; set; }
        public int QuestionTimeLimitSeconds { get; set; }
        public int TotalQuestionsPerSection { get; set; }
        public bool RandomizeQuestions { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
    }
}
