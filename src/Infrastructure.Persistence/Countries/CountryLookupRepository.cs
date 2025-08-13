// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Countries.Options;
using Infrastructure.Persistence.Countries.Sources;

namespace Infrastructure.Persistence.Countries;

internal sealed class CountryLookupRepository(IDataSource<Cca2> source, ICache cache, ExcludedCountriesOptions options) : ICountryLookupRepository
{
    private const string Key = "countries:lookup";

    public async ValueTask<Cca2[]> LookupAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(Key, out Cca2[]? cachedLookups))
            return cachedLookups;

        Cca2[] countries = await source.QueryAllAsync(cancellationToken).ConfigureAwait(false);

        Cca2[] lookups = [.. countries.Where(cca2 => !options.Excluded.Contains(cca2))];

        cache.Save(Key, lookups);
        return lookups;
    }
}
