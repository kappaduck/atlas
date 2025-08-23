// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public interface IGetCountry
{
    ValueTask<CountryResponse?> HandleAsync(string cca2, CancellationToken cancellationToken);
}

internal sealed class GetCountry(ICountryRepository repository, IStringLocalizer<Resources> localizer) : IGetCountry
{
    public async ValueTask<CountryResponse?> HandleAsync(string cca2, CancellationToken cancellationToken)
    {
        Country? country = await repository.GetAsync(new Cca2(cca2), cancellationToken).ConfigureAwait(false);

        return country?.ToResponse(localizer);
    }
}
