// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Services;

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

    public async Task<GuessedCountryResponse?> GuessAsync(string guessedCode, string code, CancellationToken cancellationToken)
    {
        Country? country = await repository.GetAsync(new Cca2(code), cancellationToken);
        Country? guessed = await repository.GetAsync(new Cca2(guessedCode), cancellationToken);

        if (country is null || guessed is null)
            return null;

        return new()
        {
            Cca2 = guessed.Cca2,
            Name = localizer[guessedCode],
            Direction = Direction.Calculate(guessed.Coordinate, country.Coordinate),
            Kilometers = (int)Math.Round(Distance.Calculate(guessed.Coordinate, country.Coordinate).Kilometers),
            IsSameContinent = guessed.Continent == country.Continent,
            Success = guessedCode == code,
            Flag = guessed.Resources.Flag
        };
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
