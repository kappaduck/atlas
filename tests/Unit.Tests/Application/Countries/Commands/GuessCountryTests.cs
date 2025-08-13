// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;

namespace Unit.Tests.Application.Countries.Commands;

internal sealed class GuessCountryTests
{
    private readonly Country _canada = CreateCanada();
    private readonly Country _italy = CreateItaly();

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();

    private readonly GuessCountry.Handler _handler;

    public GuessCountryTests()
    {
        _repository.GetAsync(_canada.Cca2, CancellationToken.None).Returns(_canada);
        _repository.GetAsync(_italy.Cca2, CancellationToken.None).Returns(_italy);

        _handler = new GuessCountry.Handler(_repository);
    }

    [Test]
    public async Task HandleShouldReturnTheGuessedCountryWhenIsNotSameCountry()
    {
        GuessCountry.Command command = new(_canada.Cca2, _italy.Cca2);

        GuessedCountryResponse guessedCountry = await _handler.Handle(command, CancellationToken.None);

        await Assert.That(guessedCountry.Cca2).IsEqualTo(_canada.Cca2);
        await Assert.That(guessedCountry.Name).IsEqualTo(_canada.Translations.First().Name);
        await Assert.That(guessedCountry.Success).IsFalse();
        await Assert.That(guessedCountry.IsSameContinent).IsFalse();
        await Assert.That(guessedCountry.Direction).IsEqualTo(104);
        await Assert.That(guessedCountry.Kilometers).IsEqualTo(6843);
        await Assert.That(guessedCountry.Flag).IsEqualTo(_canada.Resources.Flag);
    }

    [Test]
    public async Task HandleShouldReturnTheCountryWhenIsSameCountry()
    {
        GuessCountry.Command command = new(_italy.Cca2, _italy.Cca2);

        GuessedCountryResponse guessedCountry = await _handler.Handle(command, CancellationToken.None);

        await Assert.That(guessedCountry.Cca2).IsEqualTo(_italy.Cca2);
        await Assert.That(guessedCountry.Name).IsEqualTo(_italy.Translations.First().Name);
        await Assert.That(guessedCountry.Success).IsTrue();
        await Assert.That(guessedCountry.IsSameContinent).IsTrue();
        await Assert.That(guessedCountry.Direction).IsEqualTo(0);
        await Assert.That(guessedCountry.Kilometers).IsEqualTo(0);
        await Assert.That(guessedCountry.Flag).IsEqualTo(_italy.Resources.Flag);
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = new Cca2("CA"),
        Area = new Area(9984670),
        Borders = [],
        Capitals = [],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(60, -95),
        Population = 38005238,
        Translations = [new Translation(Language.English, "Canada")],
        Resources = new Resources(new Uri("https://www.google.com/maps/place/Canada"), new Uri("https://www.countryflags.io/ca/flat/64.svg"), null)
    };

    private static Country CreateItaly() => new()
    {
        Cca2 = new Cca2("IT"),
        Area = new Area(301336),
        Borders = [],
        Capitals = [],
        Continent = Continent.Europe,
        Coordinate = new Coordinate(42.83333333, 12.83333333),
        Population = 59554023,
        Translations = [new Translation(Language.English, "Italy")],
        Resources = new Resources(new Uri("https://www.google.com/maps/place/Italy"), new Uri("https://www.countryflags.io/it/flat/64.svg"), null)
    };
}
