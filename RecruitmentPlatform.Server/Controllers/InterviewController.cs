using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.DTOs;
using RecruitmentPlatform.Server.Services;

namespace RecruitmentPlatform.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InterviewController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IInterviewService _interviewService;

    public InterviewController(ApplicationDbContext context, IInterviewService interviewService)
    {
        _context = context;
        _interviewService = interviewService;
    }

    [HttpPost("start")]
    public async Task<ActionResult> StartInterview()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Email == email);

        if (application == null)
            return NotFound("Application not found");

        var session = await _interviewService.StartInterviewAsync(application.Id);
        return Ok(new { SessionId = session.Id, Message = "Interview started" });
    }

    [HttpGet("next-question")]
    public async Task<ActionResult> GetNextQuestion([FromQuery] Guid sessionId)
    {
        var question = await _interviewService.GetNextQuestionAsync(sessionId);
        if (question == null)
            return Ok(new { Message = "Interview completed or no more questions" });

        return Ok(question);
    }

    [HttpPost("submit-answer")]
    public async Task<ActionResult> SubmitAnswer([FromBody] SubmitAnswerDto dto, [FromQuery] Guid sessionId)
    {
        try
        {
            var isCorrect = await _interviewService.SubmitAnswerAsync(sessionId, dto);
            return Ok(new { IsCorrect = isCorrect });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("complete")]
    public async Task<ActionResult> CompleteInterview([FromQuery] Guid sessionId)
    {
        var result = await _interviewService.CompleteInterviewAsync(sessionId);
        if (result == null)
            return NotFound("Session not found");

        // Send result email
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (!string.IsNullOrEmpty(email))
        {
            var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
            await emailService.SendInterviewReceiptAsync(email, result);
        }

        return Ok(result);
    }

        [HttpGet("results")]
        public async Task<ActionResult> GetResults([FromQuery] Guid sessionId)
        {
            // Use interview service to get the full InterviewResultDto which includes
            // InterviewNumber, ApplicantName and SectionScores. The service will return
            // an existing result if the interview is already completed.
            var resultDto = await _interviewService.CompleteInterviewAsync(sessionId);
            if (resultDto == null)
                return NotFound("Results not available");

            return Ok(resultDto);
        }
}