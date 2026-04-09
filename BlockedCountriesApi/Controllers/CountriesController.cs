using Microsoft.AspNetCore.Mvc;
using BlockedCountriesApi.Models;
using BlockedCountriesApi.Services;
using Newtonsoft.Json.Linq;

namespace BlockedCountriesApi.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly CountryBlockService _countryService;
        private readonly TemporalBlockService _temporalService;

        public CountriesController(CountryBlockService countryService, TemporalBlockService temporalService)
        {
            _countryService = countryService;
            _temporalService = temporalService;
        }

        // POST /api/countries/block

        [HttpPost("block")]
public IActionResult BlockCountry([FromBody] CountryBlockRequest request)
{
    if (request == null || string.IsNullOrEmpty(request.CountryCode) || string.IsNullOrEmpty(request.CountryName))
        return BadRequest(new { message = "Country code and name are required." });

    var added = _countryService.Add(request.CountryCode, request.CountryName);
    if (!added)
        return Conflict(new { message = $"Country {request.CountryCode} is already blocked." });

    return Ok(new { message = $"Country {request.CountryCode} blocked successfully." });
}

[HttpPost("block/batch")]
public IActionResult BlockCountries([FromBody] List<CountryBlockRequest> request)
{
    if (request == null || request.Count == 0)
        return BadRequest(new { message = "Request cannot be empty." });

    var results = new List<object>();
    foreach (var country in request)
    {
        if (string.IsNullOrEmpty(country.CountryCode) || string.IsNullOrEmpty(country.CountryName))
        {
            results.Add(new { country = country, status = "invalid" });
            continue;
        }

        var added = _countryService.Add(country.CountryCode, country.CountryName);
        results.Add(new
        {
            country = country.CountryCode,
            status = added ? "added" : "already blocked"
        });
    }

    return Ok(results);
}


        // DELETE /api/countries/block/{countryCode}
        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode))
                return BadRequest(new { message = "Country code is required." });

            var removed = _countryService.Remove(countryCode);
            if (!removed)
                return NotFound(new { message = $"Country {countryCode} is not blocked." });

            return Ok(new { message = $"Country {countryCode} unblocked successfully." });
        }

        // GET /api/countries/blocked
        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var allCountries = _countryService.GetAll(search).ToList();

            var total = allCountries.Count;
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var items = allCountries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                TotalPages = totalPages,
                Items = items
            });
        }
        [HttpPost("temporal-block")]
public IActionResult AddTemporalBlock([FromBody] TemporalBlockRequest request)
{
    if (request == null)
        return BadRequest(new { message = "Request is required." });

    if (string.IsNullOrWhiteSpace(request.CountryCode))
        return BadRequest(new { message = "Country code is required." });

    if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
    {
        return BadRequest(new
        {
            message = "Duration must be between 1 and 1440 minutes."
        });
    }

    if (request.CountryCode.ToUpper() == "XX")
    {
        return BadRequest(new
        {
            message = "Invalid country code."
        });
    }

    var added = _temporalService.AddTemporalBlock(
        request.CountryCode,
        request.DurationMinutes
    );

    if (!added)
    {
        return Conflict(new
        {
            message = $"Country {request.CountryCode} is already temporarily blocked."
        });
    }

    return Ok(new
    {
        message = $"{request.CountryCode} blocked for {request.DurationMinutes} minutes."
    });
}
    }
}