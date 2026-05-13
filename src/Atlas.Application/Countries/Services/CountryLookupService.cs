// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Services;

internal sealed class CountryLookupService(ICountryRepository repository, IStringLocalizer<Translations> localizer) : ICountryLookupService
{
    public async Task<IEnumerable<CountryLookupResponse>> LookupAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Cca2> codes = await repository.LookupAsync(cancellationToken);
        return [.. codes.Select(Map)];
    }

    private CountryLookupResponse Map(Cca2 cca2) => new(cca2, localizer[cca2]);
}
