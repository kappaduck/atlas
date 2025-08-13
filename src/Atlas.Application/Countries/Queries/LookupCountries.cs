// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Mediator;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public static class LookupCountries
{
    public sealed record Query : IQuery<CountryLookupResponse[]>;

    internal sealed class Handler(ICountryLookupRepository repository, IStringLocalizer<Resources> localizer) : IQueryHandler<Query, CountryLookupResponse[]>
    {
        public async ValueTask<CountryLookupResponse[]> Handle(Query query, CancellationToken cancellationToken)
        {
            Cca2[] countries = await repository.LookupAsync(cancellationToken).ConfigureAwait(false);

            return [.. countries.Select(Map)];
        }

        private CountryLookupResponse Map(Cca2 cca2) => new(cca2, localizer[cca2]);
    }
}
