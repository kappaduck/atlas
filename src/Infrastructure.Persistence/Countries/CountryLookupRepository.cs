// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Countries.Sources;

namespace Infrastructure.Persistence.Countries;

internal sealed class CountryLookupRepository(IDataSource<CountryLookup> source, ICache cache) : ICountryLookupRepository
{
    private const string Key = "countries:lookup";

    public async ValueTask<CountryLookup[]> LookupAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(Key, out CountryLookup[]? cachedLookups))
            return cachedLookups;

        CountryLookup[] countries = await source.QueryAllAsync(cancellationToken).ConfigureAwait(false);

        CountryLookup[] lookups = [.. countries.Where(c => !c.IsExcluded)];

        cache.Save(Key, lookups);
        return lookups;
    }
}
