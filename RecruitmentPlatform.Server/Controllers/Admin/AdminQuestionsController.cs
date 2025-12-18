using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.DTOs.Admin;
using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Controllers.Admin;

[ApiController]
[Route("api/admin/questions")]
[Authorize(Roles = "Admin")]
public class AdminQuestionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminQuestionsController> _logger;

    public AdminQuestionsController(ApplicationDbContext context, ILogger<AdminQuestionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllQuestions()
    {
        var questions = await _context.Questions
            .OrderBy(q => q.Section)
            .ThenBy(q => q.Text)
            .Select(q => new QuestionManagementDto
            {
                Id = q.Id,
                Section = q.Section.ToString(),
                Text = q.Text,
                Options = q.Options,
                CorrectAnswerIndex = q.CorrectAnswerIndex,
                Points = q.Points,
                Explanation = q.Explanation
            })
            .ToListAsync();

        return Ok(questions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetQuestion(Guid id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question == null) return NotFound();

        return Ok(new QuestionManagementDto
        {
            Id = question.Id,
            Section = question.Section.ToString(),
            Text = question.Text,
            Options = question.Options,
            CorrectAnswerIndex = question.CorrectAnswerIndex,
            Points = question.Points,
            Explanation = question.Explanation
        });
    }

    [HttpPost]
    public async Task<ActionResult> CreateQuestion([FromBody] CreateQuestionDto dto)
    {
        if (!Enum.TryParse<QuestionSection>(dto.Section, out var section))
            return BadRequest("Invalid section");

        var question = new Question
        {
            Id = Guid.NewGuid(),
            Section = section,
            Text = dto.Text,
            Options = dto.Options,
            CorrectAnswerIndex = dto.CorrectAnswerIndex,
            Points = dto.Points,
            Explanation = dto.Explanation
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Question created: {Text}", dto.Text);
        return Ok(new { message = "Question created", id = question.Id });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateQuestion(Guid id, [FromBody] UpdateQuestionDto dto)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question == null) return NotFound();

        if (!Enum.TryParse<QuestionSection>(dto.Section, out var section))
            return BadRequest("Invalid section");

        question.Section = section;
        question.Text = dto.Text;
        question.Options = dto.Options;
        question.CorrectAnswerIndex = dto.CorrectAnswerIndex;
        question.Points = dto.Points;
        question.Explanation = dto.Explanation;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Question updated: {Id}", id);

        return Ok(new { message = "Question updated" });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteQuestion(Guid id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question == null) return NotFound();

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Question deleted: {Id}", id);
        return Ok(new { message = "Question deleted" });
    }
}