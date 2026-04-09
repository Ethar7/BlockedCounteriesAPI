using Microsoft.AspNetCore.Mvc;
using BlockedCountriesApi.Models;
using BlockedCountriesApi.Services;

[ApiController]
[Route("api/logs")]
public class LogsController : ControllerBase
{
    [HttpGet("blocked-attempts")]
    public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var skip = (page - 1) * pageSize;
        var paged = InMemoryStore.Logs.OrderByDescending(l => l.Timestamp).Skip(skip).Take(pageSize).ToList();

        return Ok(new { Page = page, PageSize = pageSize, Total = InMemoryStore.Logs.Count, Data = paged });
    }
}