// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Mediator;

namespace Atlas.Application.Countries.Commands;

public static class GuessCountry
{
    public sealed record Command(string GuessedCca2, string AnswerCca2) : ICommand<GuessedCountryResponse>;

    internal sealed class Handler(ICountryRepository repository) : ICommandHandler<Command, GuessedCountryResponse>
    {
        public async ValueTask<GuessedCountryResponse> Handle(Command command, CancellationToken cancellationToken)
        {
            Country? country = await repository.GetAsync(new Cca2(command.AnswerCca2), cancellationToken).ConfigureAwait(false);
            Country? guessedCountry = await repository.GetAsync(new Cca2(command.GuessedCca2), cancellationToken).ConfigureAwait(false);

            return Guess(country!, guessedCountry!);
        }

        private static GuessedCountryResponse Guess(Country answer, Country guessed) => new()
        {
            Cca2 = guessed.Cca2,
            Name = guessed.Translations.First(t => t.Language == Language.English).Name,
            Direction = Direction.Calculate(guessed.Coordinate, answer.Coordinate),
            Kilometers = (int)Math.Round(Distance.Calculate(guessed.Coordinate, answer.Coordinate).Kilometers),
            IsSameContinent = guessed.Continent == answer.Continent,
            Success = guessed.Cca2 == answer.Cca2,
            Flag = new ImageResponse(guessed.Resource.Flag.Uri, guessed.Resource.Flag.MediaType)
        };
    }
}
