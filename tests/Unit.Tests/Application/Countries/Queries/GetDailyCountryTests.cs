// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Services;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Unit.Tests.Application.Countries.Queries;

internal sealed class GetDailyCountryTests
{
    private readonly Country _country = CreateCanada();
    private readonly DateOnly _date = new(2025, 05, 31);

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();
    private readonly IDateHash _hash = Substitute.For<IDateHash>();
    private readonly ITimeService _timeService = Substitute.For<ITimeService>();

    private readonly GetDailyCountry.Query _query = new();
    private readonly GetDailyCountry.Handler _handler;

    public GetDailyCountryTests()
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([_country]);

        _handler = new GetDailyCountry.Handler(_repository, _hash, _timeService);
    }

    [Test]
    public async Task HandleShouldGetAllCountries()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _repository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Test]
    public async Task HandleShouldReturnNullIfNoCountriesAreAvailable()
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([]);

        CountryResponse? country = await _handler.Handle(_query, CancellationToken.None);

        await Assert.That(country).IsNull();
    }

    [Test]
    public async Task HandleShouldReturnTheCountryOfTheDay()
    {
        const uint hash = 0;

        _hash.Hash(_date).Returns(hash);

        CountryResponse? country = await _handler.Handle(_query, CancellationToken.None);

        await Assert.That(country!.Cca2).IsEqualTo(_country.Cca2);
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = new Cca2("CA"),
        Area = new Area(9984670),
        Borders = ["USA"],
        Capitals = [new Capital("Ottawa", new Coordinate(42, 42))],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(60, 95),
        Population = 38008005,
        Translations = [new Translation(Language.English, "Canada")],
        IsExcluded = false,
        Resource = new CountryResource(new Uri("https://www.google.com/maps/place/Canada"), new Image(new Uri("https://www.countryflags.io/ca/flat/64.svg"), "image/svg+xml"))
    };
}
