// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Mediator;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Queries;

public static class GetDailyCountry
{
    public sealed record Query : IQuery<CountryResponse?>;

    internal sealed class Handler(ICountryRepository repository, IStringLocalizer<Resources> localizer) : IQueryHandler<Query, CountryResponse?>
    {
        public async ValueTask<CountryResponse?> Handle(Query query, CancellationToken cancellationToken)
        {
            uint hash = Hash();

            ReadOnlySpan<Country> countries = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            if (countries.IsEmpty)
                return null;

            Country country = countries[(int)(hash % countries.Length)];

            repository.Save(country);
            return country.ToResponse(localizer);
        }

        /// <summary>
        /// Hashes the given date. The hash is deterministic and will always return the same value for the same date. It is based on the FNV-1a algorithm.
        /// https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function.
        /// </summary>
        /// <returns>The hashed value.</returns>
        private uint Hash()
        {
            const uint prime = 16777619;
            uint hash = 2166136261;

            foreach (char c in DateTime.Today.ToString("yyyyMMdd").AsSpan())
            {
                hash ^= c;
                hash *= prime;
            }

            return hash;
        }
    }
}
