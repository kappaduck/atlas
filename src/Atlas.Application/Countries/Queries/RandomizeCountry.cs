// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class RandomizeCountry
{
    public sealed record Query : IQuery<CountryResponse?>;

    internal sealed class Handler(ICountryRepository repository) : IQueryHandler<Query, CountryResponse?>
    {
        public async ValueTask<CountryResponse?> Handle(Query query, CancellationToken cancellationToken)
        {
            ReadOnlySpan<Country> countries = await repository.GetAllAsync(CancellationToken.None).ConfigureAwait(false);

            if (countries.IsEmpty)
                return null;

            Country country = countries[Random.Shared.Next(countries.Length)];

            repository.Save(country);
            return country.ToResponse();
        }
    }
}
