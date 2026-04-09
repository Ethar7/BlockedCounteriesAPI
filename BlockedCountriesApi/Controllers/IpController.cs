using Microsoft.AspNetCore.Mvc;
using BlockedCountriesApi.Models;
using BlockedCountriesApi.Services;

namespace BlockedCountriesApi.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpController : ControllerBase
    {
        private readonly IGeoLocationService _geoService;

        public IpController(IGeoLocationService geoService)
        {
            _geoService = geoService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            // Use provided IP, otherwise use caller IP
            var ip = string.IsNullOrWhiteSpace(ipAddress)
                ? HttpContext.Connection.RemoteIpAddress?.ToString()
                : ipAddress;

            // Handle localhost testing
            if (ip == "::1" || ip == "127.0.0.1" || string.IsNullOrWhiteSpace(ip))
            {
                ip = "8.8.8.8";
            }

            var result = await _geoService.LookupAsync(ip);

            if (result == null)
                return StatusCode(502, new { message = "Failed to fetch IP info." });

            return Ok(new
            {
                Ip = result.Ip,
                CountryCode = result.CountryCode,
                CountryName = result.CountryName,
                Isp = result.Isp
            });
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Handle localhost testing
            if (ip == "::1" || ip == "127.0.0.1" || string.IsNullOrWhiteSpace(ip))
            {
                ip = "8.8.8.8";
            }

            var geoResult = await _geoService.LookupAsync(ip);

            if (geoResult == null)
                return StatusCode(502, new { message = "Failed to fetch IP info." });

            var isBlocked = InMemoryStore.BlockedCountries.ContainsKey(geoResult.CountryCode);

            // Log the attempt
            InMemoryStore.Logs.Add(new BlockedAttemptLog
            {
                IpAddress = ip,
                Timestamp = DateTime.UtcNow,
                CountryCode = geoResult.CountryCode,
                IsBlocked = isBlocked,
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
            });

            var response = new IpCheckResponse
            {
                Ip = ip,
                CountryCode = geoResult.CountryCode,
                CountryName = geoResult.CountryName,
                IsBlocked = isBlocked
            };

            return Ok(response);
        }
    }
}