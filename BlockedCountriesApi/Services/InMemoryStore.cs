using System.Collections.Concurrent;
using BlockedCountriesApi.Models;

namespace BlockedCountriesApi.Services
{
    public static class InMemoryStore
    {
        public static ConcurrentDictionary<string, string> BlockedCountries { get; } =
            new ConcurrentDictionary<string, string>();

        public static ConcurrentDictionary<string, DateTime> TemporalBlockedCountries { get; } =
            new ConcurrentDictionary<string, DateTime>();

        public static ConcurrentBag<BlockedAttemptLog> Logs { get; } =
            new ConcurrentBag<BlockedAttemptLog>();
    }
}