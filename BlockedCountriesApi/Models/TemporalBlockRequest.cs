using System.ComponentModel.DataAnnotations;

namespace BlockedCountriesApi.Models;

public class TemporalBlockRequest
{
    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string CountryCode { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
    public int DurationMinutes { get; set; }
}