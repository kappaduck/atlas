// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Json;
using Infrastructure.Persistence.Countries.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Infrastructure.Persistence.Countries.Sources;

[ExcludeFromCodeCoverage]
internal sealed class CountryLookupDataSource(HttpClient client, CountryEndpointOptions options) : IDataSource<Cca2>
{
    public async Task<Cca2[]> QueryAllAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await client.GetAsync(options.Lookup, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        Cca2[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.Cca2Array, cancellationToken)
                                                           .ConfigureAwait(false);

        return countries ?? [];
    }
}
