// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Domain.Countries;
using Infrastructure.Persistence.Caching;

namespace Infrastructure.Persistence.Countries;

internal sealed class CountryRepository(ICountryClient client, ICache cache) : ICountryRepository
{
    private const string Key = "countries";
    private const string LookupKey = $"{Key}:lookup";

    public async ValueTask<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(Key, out IEnumerable<Country>? cachedCountries))
            return cachedCountries;

        IEnumerable<Country> countries = await client.GetAsync(cancellationToken);
        cache.Save(Key, countries);

        return countries;
    }

    public async ValueTask<Country?> GetAsync(Cca2 cca2, CancellationToken cancellationToken)
    {
        string key = AsKey(cca2);

        if (cache.TryGet(key, out Country? cachedCountry))
            return cachedCountry;

        Country[] countries = [.. await GetAllAsync(cancellationToken)];

        Country? country = Array.Find(countries, c => c.Cca2 == cca2);

        if (country is null)
            return null;

        cache.Save(key, country);
        return country;
    }

    public async ValueTask<IEnumerable<Cca2>> LookupAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(LookupKey, out IEnumerable<Cca2>? cachedCodes))
            return cachedCodes;

        IEnumerable<Cca2> codes = await client.LookupAsync(cancellationToken);
        cache.Save(LookupKey, codes);

        return codes;
    }

    public void Save(Country country) => cache.Save(AsKey(country.Cca2), country);

    private static string AsKey(Cca2 cca2) => $"{Key}:{cca2}";
}
