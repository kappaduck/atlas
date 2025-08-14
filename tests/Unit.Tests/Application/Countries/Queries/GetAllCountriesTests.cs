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
public sealed class GetAllCountriesTests
{
    private readonly Country _canada = CreateCanada();

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();

    private readonly GetAllCountries.Query _query;
    private readonly GetAllCountries.Handler _handler;

    public GetAllCountriesTests(LocalizerFixture localizer)
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([_canada]);

        _query = new GetAllCountries.Query();
        _handler = new GetAllCountries.Handler(_repository, localizer.Countries);
    }

    [Test]
    public async Task HandleShouldGetAllCountries()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _repository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Test]
    public async Task HandleShouldReturnAllCountries()
    {
        CountryResponse[] countries = await _handler.Handle(_query, CancellationToken.None);

        await Assert.That(countries[0].Cca2).IsEqualTo(_canada.Cca2);
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
        Resources = new Resources(new Uri("https://www.google.com/maps/place/Canada"), new Uri("https://www.countryflags.io/ca/flat/64.svg"), null)
    };
}
