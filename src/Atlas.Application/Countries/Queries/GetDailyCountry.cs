// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Services;
using Atlas.Domain.Countries;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class GetDailyCountry
{
    public sealed record Query : IQuery<CountryResponse?>;

    internal sealed class Handler(ICountryRepository repository, IDateHash dateHash, ITimeService timeService) : IQueryHandler<Query, CountryResponse?>
    {
        public async ValueTask<CountryResponse?> Handle(Query query, CancellationToken cancellationToken)
        {
            uint hash = dateHash.Hash(timeService.Today);

            ReadOnlySpan<Country> countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            if (countries.IsEmpty)
                return null;

            Country country = countries[(int)(hash % countries.Length)];

            repository.Save(country);
            return country.ToResponse();
        }
    }
}
