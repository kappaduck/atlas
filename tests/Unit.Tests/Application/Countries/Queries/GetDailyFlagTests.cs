// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Unit.Tests.Fixtures;

namespace Unit.Tests.Application.Countries.Queries;

[ClassDataSource<LocalizerFixture>]
public sealed class GetDailyFlagTests
{
    private readonly Country _country = CreateCanada();

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();

    private readonly GetDailyFlag _handler;

    public GetDailyFlagTests(LocalizerFixture localizer)
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([_country]);

        _handler = new GetDailyFlag(_repository, localizer.Countries);
    }

    [Test]
    public async Task HandleShouldGetAllCountries()
    {
        await _handler.HandleAsync(CancellationToken.None);

        await _repository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Test]
    public async Task HandleShouldReturnNullIfNoCountriesAreAvailable()
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([]);

        CountryResponse? country = await _handler.HandleAsync(CancellationToken.None);

        await Assert.That(country).IsNull();
    }

    [Test]
    public async Task HandleShouldReturnTheCountryOfTheDay()
    {
        CountryResponse? country = await _handler.HandleAsync(CancellationToken.None);

        await Assert.That(country!.Cca2).IsEqualTo(_country.Cca2);
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = new Cca2("CA"),
        Area = new Area(9984670),
        Borders = [new Cca2("US")],
        Capitals = [new Capital("Ottawa", new Coordinate(42, 42))],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(60, 95),
        Population = 38008005,
        Resources = new Resources(new Uri("https://www.google.com/maps/place/Canada"), new Uri("https://www.countryflags.io/ca/flat/64.svg"), null, new Uri("https://www.country.shape/ca.svg"))
    };
}
