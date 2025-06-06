// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Infrastructure.Persistence.Countries.Sources;

[ExcludeFromCodeCoverage]
internal sealed class CountryLookupDataSource(HttpClient client) : IDataSource<CountryLookup>
{
    public async Task<CountryLookup[]> QueryAllAsync(CancellationToken cancellationToken)
    {
        string endpoint = Path.Combine(DataJsonPaths.BaseDirectory, DataJsonPaths.LookupCountries);
        using HttpResponseMessage response = await client.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        CountryLookup[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryLookupArray, cancellationToken)
                                                         .ConfigureAwait(false);

        return countries ?? [];
    }
}
