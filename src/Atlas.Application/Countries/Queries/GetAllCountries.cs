// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class GetAllCountries
{
    public sealed record Query : IQuery<CountryResponse[]>;

    public sealed class Handler(ICountryRepository repository) : IQueryHandler<Query, CountryResponse[]>
    {
        public async ValueTask<CountryResponse[]> Handle(Query query, CancellationToken cancellationToken)
        {
            Country[] countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return [.. countries.Select(c => c.ToResponse())];
        }
    }
}
