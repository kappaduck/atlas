// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public interface IRandomizeCountry
{
    ValueTask<CountryResponse?> HandleAsync(CancellationToken cancellationToken);
}

internal sealed class RandomizeCountry(ICountryRepository repository, IStringLocalizer<Resources> localizer) : IRandomizeCountry
{
    public async ValueTask<CountryResponse?> HandleAsync(CancellationToken cancellationToken)
    {
        ReadOnlySpan<Country> countries = await repository.GetAllAsync(CancellationToken.None).ConfigureAwait(false);

        if (countries.IsEmpty)
            return null;

        Country country = countries[Random.Shared.Next(countries.Length)];

        repository.Save(country);
        return country.ToResponse(localizer);
    }
}
