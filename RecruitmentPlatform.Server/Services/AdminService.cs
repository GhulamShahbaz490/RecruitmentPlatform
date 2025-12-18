using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminService> _logger;

        public AdminService(ApplicationDbContext context, ILogger<AdminService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AdminUser?> AuthenticateAsync(string email, string password)
        {
            // In production, use Microsoft.AspNetCore.Identity with proper hashing
            var admin = await _context.AdminUsers
                .FirstOrDefaultAsync(a => a.Email == email && a.IsActive);

            if (admin == null) return null;

            // WARNING: Use proper password hashing like BCrypt or ASP.NET Identity
            // This is for demo purposes only
            bool isValid = VerifyPassword(password, admin.PasswordHash);

            if (isValid)
            {
                _logger.LogInformation("Admin logged in: {Email}", email);
                return admin;
            }

            return null;
        }

        private bool VerifyPassword(string password, string hash)
        {
            // Implement proper password verification
            // For demo, this is simplified
            return true; // Replace with actual verification
        }
    }
}
