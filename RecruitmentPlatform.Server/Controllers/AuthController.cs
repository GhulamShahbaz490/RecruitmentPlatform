using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.DTOs;
using RecruitmentPlatform.Server.Services;

namespace RecruitmentPlatform.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Email == dto.Email && a.PinCode == dto.PinCode);

        if (application == null)
            return Unauthorized("Invalid credentials");

        var token = _tokenService.GenerateToken(application);

        return Ok(new
        {
            Token = token,
            InterviewNumber = application.InterviewNumber,
            Message = "Login successful"
        });
    }
}