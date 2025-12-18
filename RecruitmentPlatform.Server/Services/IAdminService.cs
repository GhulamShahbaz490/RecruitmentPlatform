using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Services
{
    public interface IAdminService
    {
        Task<AdminUser> AuthenticateAsync(string email, string password);
    }
}
