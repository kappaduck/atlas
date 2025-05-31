// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Unit.Tests.Application.Countries.Queries;

internal sealed class GetCountryTests
{
    private readonly Country _country = CreateCanada();

    private readonly ICountryRepository _repository = Substitute.For<ICountryRepository>();

    private readonly GetCountry.Query _query;
    private readonly GetCountry.Handler _handler;

    public GetCountryTests()
    {
        _repository.GetAsync(_country.Cca2, CancellationToken.None).Returns(_country);

        _query = new GetCountry.Query(_country.Cca2);
        _handler = new GetCountry.Handler(_repository);
    }

    [Test]
    public async Task HandleShouldGetTheCountry()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _repository.Received(1).GetAsync(_country.Cca2, CancellationToken.None);
    }

    [Test]
    public async Task HandleShouldReturnTheCountry()
    {
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
