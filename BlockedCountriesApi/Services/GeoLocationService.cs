using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using BlockedCountriesApi.Models;
using Newtonsoft.Json.Linq;

namespace BlockedCountriesApi.Services
{
    public interface IGeoLocationService
    {
        Task<IpLookupResponseDto?> LookupAsync(string ip);
    }

    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeoLocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // Optional: set a reasonable timeout
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<IpLookupResponseDto?> LookupAsync(string ip)
        {
            // Validate IP format
            if (!IPAddress.TryParse(ip, out _))
                throw new ArgumentException("Invalid IP address format", nameof(ip));

            var apiKey = _configuration["GeoLocation:ApiKey"];
            var baseUrl = _configuration["GeoLocation:BaseUrl"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("GeoLocation API key is not configured in appsettings.json");

            var url = $"{baseUrl}?apiKey={apiKey}&ip={ip}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    // You can log the response.StatusCode here
                    return null; // or throw new HttpRequestException("Failed to fetch IP info");
                }

                var json = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(json);

                return new IpLookupResponseDto
                {
                    Ip = data["ip"]?.ToString() ?? ip,
                    CountryCode = data["country_code2"]?.ToString() ?? "",
                    CountryName = data["country_name"]?.ToString() ?? "",
                    Isp = data["isp"]?.ToString()
                };
            }
            catch (TaskCanceledException)
            {
                // Timeout occurred
                return null;
            }
            catch (Exception)
            {
                // General failure
                return null;
            }
        }
    }
}