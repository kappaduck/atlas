// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Providers;

internal interface ICountryProvider
{
    Task<CountryDto[]> GetAllAsync(CancellationToken cancellationToken);
}
