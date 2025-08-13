// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Countries.Options;
using Infrastructure.Persistence.Countries.Sources;

namespace Infrastructure.Persistence.Countries;

internal sealed class CountryRepository(IDataSource<Country> source, ICache cache, ExcludedCountriesOptions options) : ICountryRepository
{
    private const string Key = "countries";

    public async ValueTask<Country[]> GetAllAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(Key, out Country[]? cachedCountries))
            return cachedCountries;

        Country[] allCountries = await source.QueryAllAsync(cancellationToken).ConfigureAwait(false);

        Country[] countries = [.. allCountries.Where(c => !options.Excluded.Contains(c.Cca2))];

        cache.Save(Key, countries);
        return countries;
    }

    public async ValueTask<Country?> GetAsync(Cca2 cca2, CancellationToken cancellationToken)
    {
        string countryKey = AsKey(cca2);

        if (cache.TryGet(countryKey, out Country? cachedCountry))
            return cachedCountry;

        Country[] countries = await GetAllAsync(cancellationToken).ConfigureAwait(false);

        Country? country = Array.Find(countries, c => c.Cca2 == cca2);

        if (country is null)
            return null;

        cache.Save(countryKey, country);
        return country;
    }

    public void Save(Country country) => cache.Save(AsKey(country.Cca2), country);

    private static string AsKey(Cca2 cca2) => $"{Key}:{cca2}";
}
