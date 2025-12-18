using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentPlatform.Server.DTOs.Admin;
using RecruitmentPlatform.Server.Services;

namespace RecruitmentPlatform.Server.Controllers.Admin;

[ApiController]
[Route("api/admin/auth")]
public class AdminAuthController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AdminAuthController> _logger;

    public AdminAuthController(IAdminService adminService, ITokenService tokenService, ILogger<AdminAuthController> logger)
    {
        _adminService = adminService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] AdminLoginDto dto)
    {
        var admin = await _adminService.AuthenticateAsync(dto.Email, dto.Password);
        if (admin == null)
        {
            _logger.LogWarning("Failed admin login attempt: {Email}", dto.Email);
            return Unauthorized("Invalid credentials");
        }

        var token = _tokenService.GenerateAdminToken(admin);

        return Ok(new
        {
            Token = token,
            Email = admin.Email,
            FullName = admin.FullName,
            Message = "Admin login successful"
        });
    }
}