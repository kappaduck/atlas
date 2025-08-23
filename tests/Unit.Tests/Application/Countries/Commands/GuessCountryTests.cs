// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Microsoft.Extensions.Localization;
using Unit.Tests.Fixtures;

namespace Unit.Tests.Application.Countries.Commands;

[ClassDataSource<LocalizerFixture>]
public sealed class GuessCountryTests
{
    private readonly Country _canada = CreateCanada();
    private readonly Country _italy = CreateItaly();

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();

    private readonly GuessCountry _handler;

    public GuessCountryTests(LocalizerFixture localizer)
    {
        _repository.GetAsync(_canada.Cca2, CancellationToken.None).Returns(_canada);
        _repository.GetAsync(_italy.Cca2, CancellationToken.None).Returns(_italy);

        _handler = new GuessCountry(_repository, localizer.Countries);

        localizer.Countries[_canada.Cca2].Returns(new LocalizedString(_canada.Cca2, "Canada"));
        localizer.Countries[_italy.Cca2].Returns(new LocalizedString(_italy.Cca2, "Italy"));
    }

    [Test]
    public async Task HandleShouldReturnTheGuessedCountryWhenIsNotSameCountry()
    {
        GuessedCountryResponse guessedCountry = await _handler.HandleAsync(_canada.Cca2, _italy.Cca2, CancellationToken.None);

        await Assert.That(guessedCountry.Cca2).IsEqualTo(_canada.Cca2);
        await Assert.That(guessedCountry.Name).IsEqualTo("Canada");
        await Assert.That(guessedCountry.Success).IsFalse();
        await Assert.That(guessedCountry.IsSameContinent).IsFalse();
        await Assert.That(guessedCountry.Direction).IsEqualTo(104);
        await Assert.That(guessedCountry.Kilometers).IsEqualTo(6843);
        await Assert.That(guessedCountry.Flag).IsEqualTo(_canada.Resources.Flag);
    }

    [Test]
    public async Task HandleShouldReturnTheCountryWhenIsSameCountry()
    {
        GuessedCountryResponse guessedCountry = await _handler.HandleAsync(_italy.Cca2, _italy.Cca2, CancellationToken.None);

        await Assert.That(guessedCountry.Cca2).IsEqualTo(_italy.Cca2);
        await Assert.That(guessedCountry.Name).IsEqualTo("Italy");
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
        Resources = new Resources(new Uri("https://www.google.com/maps/place/Italy"), new Uri("https://www.countryflags.io/it/flat/64.svg"), null)
    };
}
