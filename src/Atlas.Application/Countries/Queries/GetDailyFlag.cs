// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public interface IGetDailyFlag
{
    ValueTask<CountryResponse?> HandleAsync(CancellationToken cancellationToken);
}

internal sealed class GetDailyFlag(ICountryRepository repository, IStringLocalizer<Resources> localizer) : IGetDailyFlag
{
    private const string HashKey = "flag";

    public async ValueTask<CountryResponse?> HandleAsync(CancellationToken cancellationToken)
    {
        uint hash = DateTime.Today.Hash(HashKey);

        ReadOnlySpan<Country> countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        if (countries.IsEmpty)
            return null;

        Country country = countries[(int)(hash % countries.Length)];

        repository.Save(country);
        return country.ToResponse(localizer);
    }
}
