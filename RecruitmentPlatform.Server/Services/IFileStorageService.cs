namespace RecruitmentPlatform.Server.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadResumeAsync(IFormFile file, string email);
    }
}
