using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.DTOs;
using RecruitmentPlatform.Server.Models;
using RecruitmentPlatform.Server.Services;

namespace RecruitmentPlatform.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;

    public ApplicationsController(
        ApplicationDbContext context,
        IFileStorageService fileStorage,
        IEmailService emailService,
        ITokenService tokenService)
    {
        _context = context;
        _fileStorage = fileStorage;
        _emailService = emailService;
        _tokenService = tokenService;
    }

    [HttpPost("apply")]
    public async Task<ActionResult> Apply([FromForm] RegisterApplicationDto dto)
    {
        // Validate position
        var position = await _context.Positions.FindAsync(dto.PositionId);
        if (position == null || !position.IsActive)
            return BadRequest("Invalid or closed position");

        // Check if email already applied
        var existing = await _context.Applications
            .FirstOrDefaultAsync(a => a.Email == dto.Email && a.PositionId == dto.PositionId);
        if (existing != null)
        {
            var existingToken = _tokenService.GenerateToken(existing);
            return Ok(new
            {
                Message = "Already applied for this position. Logging you in.",
                InterviewNumber = existing.InterviewNumber,
                Token = existingToken
            });
        }

        // Upload resume
        var resumeUrl = await _fileStorage.UploadResumeAsync(dto.Resume, dto.Email);

        // Generate credentials
        var interviewNumber = GenerateInterviewNumber();
        var pinCode = GeneratePinCode();

        var application = new Application
        {
            Id = Guid.NewGuid(),
            PositionId = dto.PositionId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            ResumeUrl = resumeUrl,
            InterviewNumber = interviewNumber,
            PinCode = pinCode
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        // Send email
        //await _emailService.SendInterviewCredentialsAsync(dto.Email, interviewNumber, pinCode);

        // Generate JWT for immediate login
        var token = _tokenService.GenerateToken(application);

        return Ok(new
        {
            Message = "Application submitted successfully",
            InterviewNumber = interviewNumber,
            Token = token
        });
    }

    [HttpGet("my-applications")]
    [Authorize]
    public async Task<ActionResult> GetMyApplications()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var applications = await _context.Applications
            .Where(a => a.Email == email)
            .Include(a => a.Position)
            .Include(a => a.InterviewSessions)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new
            {
                a.Id,
                a.InterviewNumber,
                PositionTitle = a.Position.Title,
                a.Position.TechStack,
                AppliedAt = a.AppliedAt.ToString("yyyy-MM-dd"),
                InterviewStatus = a.InterviewSessions.Any(s => s.Status == InterviewStatus.Completed)
                    ? "Completed"
                    : "Pending",
                CanStartInterview = !a.InterviewSessions.Any(s => s.Status != InterviewStatus.NotStarted),
                LatestSessionId = a.InterviewSessions
                    .OrderByDescending(s => s.CreatedAt)
                    .Select(s => s.Id)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(applications);
    }

    private string GenerateInterviewNumber()
    {
        var datePart = DateTime.UtcNow.ToString("yyMMdd");
        var randomPart = new Random().Next(1000, 9999);
        return $"INT-{datePart}-{randomPart}";
    }

    private string GeneratePinCode()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}
