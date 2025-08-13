// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Json;
using Infrastructure.Persistence.Countries.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Infrastructure.Persistence.Countries.Sources;

[ExcludeFromCodeCoverage]
internal sealed class CountryDataSource(HttpClient client, CountryEndpointOptions options) : IDataSource<Country>
{
    public async Task<Country[]> QueryAllAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await client.GetAsync(options.All, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        Country[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryArray, cancellationToken)
                                                     .ConfigureAwait(false);

        return countries ?? [];
    }
}
