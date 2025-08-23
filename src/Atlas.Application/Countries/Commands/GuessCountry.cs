// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries.Commands;

public interface IGuessCountry
{
    ValueTask<GuessedCountryResponse> HandleAsync(string guessedCca2, string answerCca2, CancellationToken cancellationToken);
}

internal sealed class GuessCountry(ICountryRepository repository, IStringLocalizer<Resources> localizer) : IGuessCountry
{
    public async ValueTask<GuessedCountryResponse> HandleAsync(string guessedCca2, string answerCca2, CancellationToken cancellationToken)
    {
        Country? country = await repository.GetAsync(new Cca2(answerCca2), cancellationToken).ConfigureAwait(false);
        Country? guessedCountry = await repository.GetAsync(new Cca2(guessedCca2), cancellationToken).ConfigureAwait(false);

        return Guess(country!, guessedCountry!);
    }

    private GuessedCountryResponse Guess(Country answer, Country guessed) => new()
    {
        Cca2 = guessed.Cca2,
        Name = localizer[guessed.Cca2],
        Direction = Direction.Calculate(guessed.Coordinate, answer.Coordinate),
        Kilometers = (int)Math.Round(Distance.Calculate(guessed.Coordinate, answer.Coordinate).Kilometers),
        IsSameContinent = guessed.Continent == answer.Continent,
        Success = guessed.Cca2 == answer.Cca2,
        Flag = guessed.Resources.Flag
    };
}
