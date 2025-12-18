using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Controllers.Admin;

[ApiController]
[Route("api/admin/positions")]
[Authorize(Roles = "Admin")]
public class AdminPositionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminPositionsController> _logger;

    public AdminPositionsController(ApplicationDbContext context, ILogger<AdminPositionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPositions()
    {
        var positions = await _context.Positions
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return Ok(positions);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePosition([FromBody] Position position)
    {
        position.Id = Guid.NewGuid();
        position.CreatedAt = DateTime.UtcNow;
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Position created: {Title}", position.Title);
        return CreatedAtAction(nameof(GetPosition), new { id = position.Id }, position);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePosition(Guid id, [FromBody] Position updatedPosition)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null) return NotFound();

        position.Title = updatedPosition.Title;
        position.Description = updatedPosition.Description;
        position.Level = updatedPosition.Level;
        position.TechStack = updatedPosition.TechStack;
        position.Location = updatedPosition.Location;
        position.IsActive = updatedPosition.IsActive;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Position updated: {Id}", id);

        return Ok(position);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePosition(Guid id)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null) return NotFound();

        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Position deleted: {Id}", id);
        return Ok(new { message = "Position deleted successfully" });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPosition(Guid id)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null) return NotFound();

        return Ok(position);
    }
}