// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Infrastructure.Persistence.Countries;

internal interface ICountryClient
{
    Task<IEnumerable<Country>> GetAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Cca2>> LookupAsync(CancellationToken cancellationToken);
}
