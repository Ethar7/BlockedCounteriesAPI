using System.ComponentModel.DataAnnotations;

namespace BlockedCountriesApi.Models
{
    public class CountryBlockRequest
    {
        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be 2 characters.")]
        public string CountryCode { get; set; } = string.Empty;

        public string? CountryName { get; set; }
    }
}