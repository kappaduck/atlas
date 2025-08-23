// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public interface IGetAllCountries
{
    ValueTask<CountryResponse[]> HandleAsync(CancellationToken cancellationToken);
}

internal sealed class GetAllCountries(ICountryRepository repository, IStringLocalizer<Resources> localizer) : IGetAllCountries
{
    public async ValueTask<CountryResponse[]> HandleAsync(CancellationToken cancellationToken)
    {
        Country[] countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return [.. countries.Select(c => c.ToResponse(localizer))];
    }
}
