using RecruitmentPlatform.Server.DTOs;
using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Services
{
    public interface IInterviewService
    {
        Task<InterviewQuestionDto> GetNextQuestionAsync(Guid interviewSessionId);
        Task<bool> SubmitAnswerAsync(Guid interviewSessionId, SubmitAnswerDto answerDto);
        Task<InterviewResultDto> CompleteInterviewAsync(Guid interviewSessionId);
        Task<InterviewSession> StartInterviewAsync(Guid applicationId);
    }
}
