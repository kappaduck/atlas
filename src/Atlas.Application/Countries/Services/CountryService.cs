// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Services;

internal sealed class CountryService(ICountryRepository repository, IStringLocalizer<Translations> localizer) : ICountryService
{
    public async Task<IEnumerable<CountryListItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Country> countries = await repository.GetAllAsync(cancellationToken);
        return [.. countries.Select(ToItem)];

        CountryListItem ToItem(Country country)
            => new(localizer[country.Cca2], localizer[country.Continent.ToString()], country.Resources.Map, country.Resources.Flag);
    }

    public async Task<CountryResponse> GetAsync(string code, CancellationToken cancellationToken)
    {
        Country country = await repository.GetAsync(new Cca2(code), cancellationToken).ConfigureAwait(false);
        return country.ToResponse(localizer);
    }

    public async Task<CountryResponse> GetDailyCountryAsync(CancellationToken cancellationToken)
    {
        ReadOnlySpan<Country> countries = [.. await repository.GetAllAsync(cancellationToken)];

        int index = DateTime.Today.HashedIndex("country", countries.Length);
        Country country = countries[index];

        repository.Save(country);
        return country.ToResponse(localizer);
    }

    public async Task<CountryResponse> GetDailyFlagAsync(CancellationToken cancellationToken)
    {
        ReadOnlySpan<Country> countries = [.. await repository.GetAllAsync(cancellationToken)];

        int index = DateTime.Today.HashedIndex("flag", countries.Length);
        Country country = countries[index];

        repository.Save(country);
        return country.ToResponse(localizer);
    }

    public async Task<GuessedResponse> GuessAsync(string guessedCode, string code, CancellationToken cancellationToken)
    {
        Country country = await repository.GetAsync(new Cca2(code), cancellationToken);
        Country guessed = await repository.GetAsync(new Cca2(guessedCode), cancellationToken);

        if (guessedCode == code)
        {
            return new GoodGuessResponse()
            {
                Cca2 = guessed.Cca2,
                Name = localizer[guessedCode],
                Continent = localizer[guessed.Continent.ToString()],
                Flag = guessed.Resources.Flag,
                Map = guessed.Resources.Map
            };
        }

        Distance distance = Distance.Calculate(guessed.Coordinate, country.Coordinate);
        return new WrongGuessResponse()
        {
            Cca2 = guessed.Cca2,
            Name = localizer[guessedCode],
            Direction = Direction.Calculate(guessed.Coordinate, country.Coordinate),
            Proximity = Proximity.Calculate(guessed.Coordinate, country.Coordinate),
            Kilometers = (int)Math.Round(distance.Kilometers),
            Miles = (int)Math.Round(distance.Miles),
            Continent = localizer[guessed.Continent.ToString()],
            IsSameContinent = guessed.Continent == country.Continent,
            Flag = guessed.Resources.Flag,
            Map = guessed.Resources.Map
        };
    }

    public async Task<CountryResponse> RandomizeAsync(CancellationToken cancellationToken)
    {
        ReadOnlySpan<Country> countries = [.. await repository.GetAllAsync(cancellationToken)];

        Country country = countries[Random.Shared.Next(countries.Length)];

        repository.Save(country);
        return country.ToResponse(localizer);
    }
}
