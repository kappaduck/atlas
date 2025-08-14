// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Mediator;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public static class GetCountry
{
    public sealed record Query(string Cca2) : IQuery<CountryResponse?>;

    internal sealed class Handler(ICountryRepository repository, IStringLocalizer<Resources> localizer) : IQueryHandler<Query, CountryResponse?>
    {
        public async ValueTask<CountryResponse?> Handle(Query query, CancellationToken cancellationToken)
        {
            Country? country = await repository.GetAsync(new Cca2(query.Cca2), cancellationToken).ConfigureAwait(false);

            return country?.ToResponse(localizer);
        }
    }
}
