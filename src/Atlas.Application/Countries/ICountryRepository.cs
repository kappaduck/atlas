// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Atlas.Application.Countries;

public interface ICountryRepository
{
    void Save(Country country);

    ValueTask<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken);

    ValueTask<Country?> GetAsync(Cca2 cca2, CancellationToken cancellationToken);

    ValueTask<IEnumerable<Cca2>> LookupAsync(CancellationToken cancellationToken);
}
