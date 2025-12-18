using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Services
{
    public interface ITokenService
    {
        string GenerateToken(Application application);
        string GenerateAdminToken(AdminUser admin);
    }
}
