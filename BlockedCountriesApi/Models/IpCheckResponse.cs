namespace BlockedCountriesApi.Models
{
    public class IpCheckResponse
    {
        public string Ip { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public bool IsBlocked { get; set; }
    }
}