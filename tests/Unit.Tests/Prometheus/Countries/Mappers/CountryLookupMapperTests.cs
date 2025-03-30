// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;
using Prometheus.Countries.Mappers;

namespace Unit.Tests.Prometheus.Countries.Mappers;

public sealed class CountryLookupMapperTests
{
    private readonly Country _canada = CreateCanada();

    [Test]
    public async Task AsLookupsShouldMapDomainToCountryLookup()
    {
        Country[] countries = [_canada];

        CountryLookup[] countryLookups = countries.AsLookups();

        CountryLookup country = countryLookups[0];

        await Assert.That(country.Cca2).IsEqualTo(_canada.Cca2);
        await Assert.That(country.Translations).Contains(t => t.Language == Language.French && t.Name == "Canada");
        await Assert.That(country.IsExcluded).IsFalse();
    }

    private static Country CreateCanada() => new()
    {
        Cca2 = new Cca2("CA"),
        Capitals = [new Capital("Ottawa", new Coordinate(1, 2))],
        Area = new Area(9984670),
        Population = 36624199,
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(56.1304, -106.3468),
        Borders = ["USA"],
        Translations = [new Translation(Language.French, "Canada")],
        IsExcluded = false,
        Resource = new CountryResource(new Uri("https://www.google.com/maps/place/Canada"), new Image(new Uri("https://www.countryflags.io/ca/flat/64.png"), "image/png"))
    };
}
