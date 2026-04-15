// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Services;

public interface ICountryService
{
    Task<IEnumerable<CountryResponse>> GetAllAsync(CancellationToken cancellationToken);

    Task<CountryResponse?> GetAsync(string code, CancellationToken cancellationToken);

    Task<CountryResponse?> GetDailyCountryAsync(CancellationToken cancellationToken);

    Task<CountryResponse?> GetDailyFlagAsync(CancellationToken cancellationToken);

    Task<CountryResponse?> RandomizeAsync(CancellationToken cancellationToken);
}

internal sealed class CountryService(ICountryRepository repository, IStringLocalizer<Translations> localizer) : ICountryService
{
    public async Task<IEnumerable<CountryResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Country> countries = await repository.GetAllAsync(cancellationToken);
        return [.. countries.Select(c => c.ToResponse(localizer))];
    }

    public async Task<CountryResponse?> GetAsync(string code, CancellationToken cancellationToken)
    {
        Country? country = await repository.GetAsync(new Cca2(code), cancellationToken).ConfigureAwait(false);
        return country?.ToResponse(localizer);
    }

    public async Task<CountryResponse?> GetDailyCountryAsync(CancellationToken cancellationToken)
    {
        ReadOnlySpan<Country> countries = [.. await repository.GetAllAsync(cancellationToken)];

        if (countries.IsEmpty)
            return null;

        int hash = DateTime.Today.Hash("flag");
        Country country = countries[hash % countries.Length];

        repository.Save(country);
        return country.ToResponse(localizer);
    }

    public async Task<CountryResponse?> GetDailyFlagAsync(CancellationToken cancellationToken)
    {
        ReadOnlySpan<Country> countries = [.. await repository.GetAllAsync(cancellationToken)];

        if (countries.IsEmpty)
            return null;

        int hash = DateTime.Today.Hash("country");
        Country country = countries[hash % countries.Length];

        repository.Save(country);
        return country.ToResponse(localizer);
    }

    public async Task<CountryResponse?> RandomizeAsync(CancellationToken cancellationToken)
    {
        ReadOnlySpan<Country> countries = [.. await repository.GetAllAsync(cancellationToken)];

        if (countries.IsEmpty)
            return null;

        Country country = countries[Random.Shared.Next(countries.Length)];

        repository.Save(country);
        return country.ToResponse(localizer);
    }
}
