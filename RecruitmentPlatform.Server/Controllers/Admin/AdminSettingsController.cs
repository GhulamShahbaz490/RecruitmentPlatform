using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.DTOs.Admin;
using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Controllers.Admin;

[ApiController]
[Route("api/admin/settings")]
[Authorize(Roles = "Admin")]
public class AdminSettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminSettingsController> _logger;

    public AdminSettingsController(ApplicationDbContext context, ILogger<AdminSettingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetSettings()
    {
        var settings = await _context.InterviewSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new InterviewSettings { Id = Guid.NewGuid() };
            _context.InterviewSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        return Ok(new SettingsDto
        {
            PassThresholdPercentage = settings.PassThresholdPercentage,
            QuestionTimeLimitSeconds = settings.QuestionTimeLimitSeconds,
            TotalQuestionsPerSection = settings.TotalQuestionsPerSection,
            RandomizeQuestions = settings.RandomizeQuestions,
            EmailNotificationsEnabled = settings.EmailNotificationsEnabled
        });
    }

    [HttpPut]
    public async Task<ActionResult> UpdateSettings([FromBody] SettingsDto dto)
    {
        var settings = await _context.InterviewSettings.FirstOrDefaultAsync();
        if (settings == null) return NotFound();

        settings.PassThresholdPercentage = dto.PassThresholdPercentage;
        settings.QuestionTimeLimitSeconds = dto.QuestionTimeLimitSeconds;
        settings.TotalQuestionsPerSection = dto.TotalQuestionsPerSection;
        settings.RandomizeQuestions = dto.RandomizeQuestions;
        settings.EmailNotificationsEnabled = dto.EmailNotificationsEnabled;
        settings.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Interview settings updated");

        return Ok(new { message = "Settings updated successfully" });
    }
}