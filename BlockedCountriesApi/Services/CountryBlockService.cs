using BlockedCountriesApi.Models;

namespace BlockedCountriesApi.Services
{
    public class CountryBlockService
    {
        public bool Add(string code, string name)
        {
            return InMemoryStore.BlockedCountries.TryAdd(code.ToUpper(), name);
        }

        public bool Remove(string code)
        {
            return InMemoryStore.BlockedCountries.TryRemove(code.ToUpper(), out _);
        }

        public IEnumerable<CountryBlockRequest> GetAll(string? search = null)
        {
            var list = InMemoryStore.BlockedCountries
                .Select(kv => new CountryBlockRequest { CountryCode = kv.Key, CountryName = kv.Value });

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper();
                list = list.Where(c => c.CountryCode.Contains(search) || c.CountryName.ToUpper().Contains(search));
            }

            return list;
        }
    }
}