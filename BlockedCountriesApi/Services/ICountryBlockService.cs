using System.Collections.Generic;

namespace BlockedCountriesApi.Services;

public interface ICountryBlockService
{
    bool AddBlockedCountry(string countryCode);
    bool RemoveBlockedCountry(string countryCode);
    bool IsBlocked(string countryCode);
    IEnumerable<string> GetBlockedCountries();
}