// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Atlas.Application.Countries.Services;

public interface ICountryLookupService
{
    Task<IEnumerable<CountryLookupResponse>> LookupAsync(CancellationToken cancellationToken);
}
