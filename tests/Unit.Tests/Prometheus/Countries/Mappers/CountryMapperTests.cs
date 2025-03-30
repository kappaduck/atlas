// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Atlas.Domain.Languages;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Mappers;
using System.Net.Mime;

namespace Unit.Tests.Prometheus.Countries.Mappers;

public sealed class CountryMapperTests
{
    private readonly CountryDto _dto = new()
    {
        Cca2 = "CA",
        Name = new NameDto("Canada"),
        Capitals = ["Ottawa"],
        Borders = ["USA"],
        Area = 9984670,
        Population = 38041000,
        Coordinate = new CoordinateDto(60.0, 95.0),
        Region = RegionDto.Americas,
        SubRegion = SubRegionDto.NorthAmerica,
        CapitalInfo = new CapitalInfoDto { Coordinate = new CoordinateDto(45.4215, 75.6972) },
        Translations = [new TranslationDto("fra", "Canada")],
        Maps = new MapsDto(new Uri("https://www.google.com/maps/place/Canada")),
        Flags = new FlagsDto(new Uri("https://www.countryflags.io/ca/flat/64.png"))
    };

    [Test]
    public async Task AsDomainShouldMapDtoToDomain()
    {
        CountryDto[] dtos = [_dto];

        Country[] countries = dtos.AsDomain(["fra", "eng"], []);

        Country country = countries[0];

        using (Assert.Multiple())
        {
            await Assert.That(country.Cca2.Code).IsEqualTo("CA");
            await Assert.That(country.Borders).IsEqualTo(_dto.Borders);
            await Assert.That(country.Population).IsEqualTo(_dto.Population);
            await Assert.That(country.Continent).IsEqualTo(Continent.NorthAmerica);

            double area = country.Area;
            await Assert.That(area).IsEqualTo(_dto.Area);

            await Assert.That(country.Coordinate.Latitude).IsEqualTo(_dto.Coordinate.Latitude);
            await Assert.That(country.Coordinate.Longitude).IsEqualTo(_dto.Coordinate.Longitude);

            Capital capital = country.Capitals.First();
            await Assert.That(capital.Name).IsEqualTo("Ottawa");
            await Assert.That(capital.Coordinate.Latitude).IsEqualTo(_dto.CapitalInfo.Coordinate!.Latitude);
            await Assert.That(capital.Coordinate.Longitude).IsEqualTo(_dto.CapitalInfo.Coordinate.Longitude);

            await Assert.That(country.Translations).Contains(t => t.Language == Language.French && t.Name == "Canada");
            await Assert.That(country.Translations).Contains(t => t.Language == Language.English && t.Name == "Canada");

            await Assert.That(country.IsExcluded).IsFalse();

            await Assert.That(country.Resource.Map).IsEqualTo(new Uri("https://www.google.com/maps/place/Canada"));
            await Assert.That(country.Resource.Flag.Uri).IsEqualTo(new Uri("https://www.countryflags.io/ca/flat/64.png"));
            await Assert.That(country.Resource.Flag.MediaType).IsEqualTo(MediaTypeNames.Image.Svg);
        }
    }

    [Test]
    public async Task DtoWhichIsExcludedShouldMapToDomainWithIsExcluded()
    {
        CountryDto[] dtos = [_dto];

        Country[] countries = dtos.AsDomain(["fra", "eng"], [_dto.Cca2]);

        Country country = countries[0];

        await Assert.That(country.IsExcluded).IsTrue();
    }

    [Test]
    public async Task DtoWithoutBordersShouldMapToEmptyArray()
    {
        CountryDto countryWithoutBorder = _dto with { Borders = null };

        CountryDto[] dtos = [countryWithoutBorder];

        Country[] countries = dtos.AsDomain(["fra", "eng"], []);

        Country country = countries[0];

        await Assert.That(country.Borders).IsEmpty();
    }
}
