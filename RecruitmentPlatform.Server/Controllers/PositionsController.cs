using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> GetActivePositions()
    {
        var positions = await _context.Positions
            .Where(p => p.IsActive)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Description,
                Level = p.Level.ToString(),
                p.TechStack,
                p.Location,
                p.CreatedAt
            })
            .ToListAsync();

        return Ok(positions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPosition(Guid id)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null) return NotFound();

        return Ok(new
        {
            position.Id,
            position.Title,
            position.Description,
            Level = position.Level.ToString(),
            position.TechStack,
            position.Location
        });
    }

    // Admin endpoints
    [HttpPost]
    public async Task<ActionResult> PostPosition([FromBody] Position position)
    {
        position.Id = Guid.NewGuid();
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPosition), new { id = position.Id }, position);
    }
}
