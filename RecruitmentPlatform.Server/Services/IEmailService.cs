using RecruitmentPlatform.Server.DTOs;

namespace RecruitmentPlatform.Server.Services
{
    public interface IEmailService
    {
        Task SendInterviewCredentialsAsync(string toEmail, string interviewNumber, string pinCode);
        Task SendInterviewReceiptAsync(string toEmail, InterviewResultDto result);
    }
}
