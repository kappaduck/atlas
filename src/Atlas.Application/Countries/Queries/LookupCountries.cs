// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public interface ILookupCountries
{
    ValueTask<CountryLookupResponse[]> HandleAsync(CancellationToken cancellationToken);
}

internal sealed class LookupCountries(ICountryLookupRepository repository, IStringLocalizer<Resources> localizer) : ILookupCountries
{
    public async ValueTask<CountryLookupResponse[]> HandleAsync(CancellationToken cancellationToken)
    {
        Cca2[] countries = await repository.LookupAsync(cancellationToken).ConfigureAwait(false);

        return [.. countries.Select(Map)];
    }

    private CountryLookupResponse Map(Cca2 cca2) => new(cca2, localizer[cca2]);
}
