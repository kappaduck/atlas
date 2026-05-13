// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Json;
using System.Net.Http.Json;

namespace Infrastructure.Persistence.Countries;

internal class CountryClient(HttpClient http, CountryEndpointOptions options) : ICountryClient
{
    public async Task<IEnumerable<Country>> GetAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await http.GetAsync(options.All, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return [];

        Country[]? countries = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryArray, cancellationToken);
        return countries ?? [];
    }

    public async Task<IEnumerable<Cca2>> LookupAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await http.GetAsync(options.Lookup, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return [];

        Cca2[]? codes = await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.Cca2Array, cancellationToken);
        return codes ?? [];
    }
}
