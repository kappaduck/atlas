// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Json;

namespace Infrastructure.Persistence.Countries;

internal class CountryClient(HttpClient http, CountryEndpointOptions options) : ICountryClient
{
    public async Task<IEnumerable<Country>> GetAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await http.GetAsync(options.All, cancellationToken);
        response.ThrowIfFailed();

        return (await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.CountryArray, cancellationToken))!;
    }

    public async Task<IEnumerable<Cca2>> LookupAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await http.GetAsync(options.Lookup, cancellationToken);
        response.ThrowIfFailed();

        return (await response.Content.ReadFromJsonAsync(CountryJsonContext.Default.Cca2Array, cancellationToken))!;
    }
}
