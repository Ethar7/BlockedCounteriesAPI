using BlockedCountriesApi.Models;

namespace BlockedCountriesApi.Services
{
    public class TemporalBlockService
    {
        public bool AddTemporalBlock(string countryCode, int durationMinutes)
        {
            var code = countryCode.ToUpper();

            if (InMemoryStore.TemporalBlockedCountries.ContainsKey(code))
                return false;

            var expiryTime = DateTime.UtcNow.AddMinutes(durationMinutes);

            return InMemoryStore.TemporalBlockedCountries
                .TryAdd(code, expiryTime);
        }

        public void RemoveExpiredBlocks()
        {
            var now = DateTime.UtcNow;

            foreach (var item in InMemoryStore.TemporalBlockedCountries)
            {
                if (item.Value <= now)
                {
                    InMemoryStore.TemporalBlockedCountries
                        .TryRemove(item.Key, out _);
                }
            }
        }
    }
}