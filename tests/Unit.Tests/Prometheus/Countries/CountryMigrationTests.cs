// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Infrastructure.Json;
using Microsoft.Extensions.Logging;
using Prometheus.Countries;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Options;
using Prometheus.Countries.Providers;
using Prometheus.Files;
using Prometheus.Patch;
using System.Text.Json.Serialization.Metadata;

namespace Unit.Tests.Prometheus.Countries;

internal sealed class CountryMigrationTests
{
    private const string Path = "path";
    private const string CountriesPath = $"{Path}/{DataJsonPaths.Countries}";

    private readonly CountryDto _canada = CreateCanada();
    private readonly CountryFilterOptions _options = new()
    {
        Languages = ["fra", "eng"],
        ExcludedCountries = []
    };

    private readonly ICountryProvider _provider = Substitute.For<ICountryProvider>();
    private readonly IJsonFileWriter _writer = Substitute.For<IJsonFileWriter>();
    private readonly IPatch<Span<CountryDto>> _patch = new CountryPatch();

    private readonly CountryMigration _migration;

    public CountryMigrationTests()
    {
        _provider.GetAllAsync(CancellationToken.None).Returns([_canada]);

        ILogger<CountryMigration> logger = Substitute.For<ILogger<CountryMigration>>();

        _migration = new CountryMigration(_provider, _writer, _patch, logger, _options);
    }

    [Test]
    public async Task MigrateAsyncShouldGetTheData()
    {
        await _migration.MigrateAsync(Path, CancellationToken.None);

        await _provider.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Test]
    public async Task MigrateAsyncShouldWriteToCountriesFile()
    {
        await _migration.MigrateAsync(Path, CancellationToken.None);

        await _writer.Received(1).WriteToAsync(CountriesPath, Arg.Is<Country[]>(c => c.Any(x => x.Cca2 == _canada.Cca2)), Arg.Any<JsonTypeInfo<Country[]>>(), CancellationToken.None);
    }

    [Test]
    public async Task MigrateAsyncShouldOnlyAcceptSomeTranslations()
    {
        await _migration.MigrateAsync(Path, CancellationToken.None);

        await _writer.Received(1).WriteToAsync(Arg.Any<string>(), Arg.Is<Country[]>(c => c.Any(x => x.Translations.Any(t => t.Language == Language.French || t.Language == Language.English))), Arg.Any<JsonTypeInfo<Country[]>>(), CancellationToken.None);
    }

    [Test]
    public async Task MigrateAsyncShouldHaveCountryWithIsExcludedWhenCountryIsExcluded()
    {
        _options.ExcludedCountries = ["CA"];

        await _migration.MigrateAsync(Path, CancellationToken.None);

        await _writer.Received(1).WriteToAsync(CountriesPath, Arg.Is<Country[]>(c => c.Any(x => x.IsExcluded)), Arg.Any<JsonTypeInfo<Country[]>>(), CancellationToken.None);
    }

    [Test]
    public async Task MigrateAsyncShouldWriteToLookupCountriesFile()
    {
        const string searchCountriesPath = $"{Path}/{DataJsonPaths.LookupCountries}";

        await _migration.MigrateAsync(Path, CancellationToken.None);

        await _writer.Received(1).WriteToAsync(searchCountriesPath, Arg.Is<CountryLookup[]>(c => c.Any(x => x.Cca2 == _canada.Cca2)), Arg.Any<JsonTypeInfo<CountryLookup[]>>(), CancellationToken.None);
    }

    [Test]
    public async Task MigrateAsyncShouldNotWriteToFilesWhenThereIsNoCountries()
    {
        _provider.GetAllAsync(CancellationToken.None).Returns([]);

        await _migration.MigrateAsync(Path, CancellationToken.None);

        await _writer.DidNotReceiveWithAnyArgs().WriteToAsync<Arg.AnyType>(default!, default!, default!, CancellationToken.None);
    }

    private static CountryDto CreateCanada() => new()
    {
        Cca2 = "CA",
        Capitals = ["Ottawa"],
        Area = 9984670,
        Population = 36624199,
        Name = new NameDto("Canada"),
        CapitalInfo = new CapitalInfoDto() { Coordinate = new CoordinateDto(45.4215, -75.6972) },
        Coordinate = new CoordinateDto(56.1304, -106.3468),
        Region = RegionDto.Americas,
        SubRegion = SubRegionDto.NorthAmerica,
        Borders = ["USA"],
        Translations = [new TranslationDto("fra", "Canada"), new TranslationDto("eng", "Canada"), new TranslationDto("deu", "Kanada")],
        Maps = new MapsDto(new Uri("https://example.com/maps.com")),
        Flags = new FlagsDto(new Uri("https://example.com/flags.com"))
    };
}
