// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Atlas.Application.Countries.Services;

public interface ICountryService
{
    Task<IEnumerable<CountryResponse>> GetAllAsync(CancellationToken cancellationToken);

    Task<CountryResponse?> GetAsync(string code, CancellationToken cancellationToken);

    Task<CountryResponse?> GetDailyCountryAsync(CancellationToken cancellationToken);

    Task<CountryResponse?> GetDailyFlagAsync(CancellationToken cancellationToken);

    Task<GuessedCountryResponse?> GuessAsync(string guessedCode, string code, CancellationToken cancellationToken);

    Task<CountryResponse?> RandomizeAsync(CancellationToken cancellationToken);
}
