using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Data;
using System.Linq;

namespace RecruitmentPlatform.Server.Controllers.Admin;

[ApiController]
[Route("api/admin/candidates")]
[Authorize(Roles = "Admin")]
public class AdminCandidatesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminCandidatesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/admin/candidates
    // Optional query params: status (Pass/Fail), minPercentage, search (email/name/position)
    [HttpGet]
    public async Task<ActionResult> GetCandidates([FromQuery] string? status, [FromQuery] double? minPercentage, [FromQuery] string? search)
    {
        var query = _context.Applications
            .Include(a => a.Position)
            .Include(a => a.InterviewSessions)
                .ThenInclude(s => s.Result)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(a => a.Email.ToLower().Contains(s) || a.FirstName.ToLower().Contains(s) || a.LastName.ToLower().Contains(s) || a.Position.Title.ToLower().Contains(s));
        }

        var list = await query
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new {
                a.Id,
                a.FirstName,
                a.LastName,
                a.Email,
                PositionTitle = a.Position.Title,
                InterviewNumber = a.InterviewNumber,
                AppliedAt = a.AppliedAt,
                LatestSession = a.InterviewSessions
                    .OrderByDescending(s => s.CreatedAt)
                    .Select(s => new {
                        s.Id,
                        Status = s.Status.ToString(),
                        Result = s.Result == null ? null : new {
                            s.Result.TotalScore,
                            s.Result.MaxPossibleScore,
                            s.Result.Percentage,
                            Status = s.Result.Status.ToString()
                        }
                    })
                    .FirstOrDefault()
            })
            .ToListAsync();

        // Apply server-side filters (operate in-memory)
        var filtered = list.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(status))
        {
            var st = status.Trim();
            filtered = filtered.Where(x => (x.LatestSession?.Result?.Status ?? string.Empty) == st || (x.LatestSession?.Status ?? string.Empty) == st);
        }

        if (minPercentage.HasValue)
        {
            filtered = filtered.Where(x => (double?)(x.LatestSession?.Result?.Percentage) >= minPercentage.Value);
        }

        return Ok(filtered);
    }
}
