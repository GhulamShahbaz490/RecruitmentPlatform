namespace RecruitmentPlatform.Server.DTOs
{
    public class InterviewResultDto
    {
        public string InterviewNumber { get; set; } = string.Empty;
        public string ApplicantName { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalScore { get; set; }
        public int MaxPossibleScore { get; set; }
        public Dictionary<string, SectionScoreDto> SectionScores { get; set; } = new();
    }
}
