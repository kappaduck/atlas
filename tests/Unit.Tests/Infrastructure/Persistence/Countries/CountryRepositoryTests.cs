// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Geography;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Countries;
using Infrastructure.Persistence.Countries.Options;
using Infrastructure.Persistence.Countries.Sources;

namespace Unit.Tests.Infrastructure.Persistence.Countries;

public sealed class CountryRepositoryTests
{
    private const string ExpectedAllCountriesKey = "countries";

    private readonly IDataSource<Country> _dataSource = Substitute.For<IDataSource<Country>>();
    private readonly ICache _cache = Substitute.For<ICache>();

    private readonly CountryRepository _repository;

    public CountryRepositoryTests()
    {
        _repository = new CountryRepository(_dataSource, _cache, new ExcludedCountriesOptions()
        {
            Excluded = ["IT"]
        });
    }

    [Test]
    public void SaveShouldCacheCountry()
    {
        Country country = CreateCountry("CA");
        string expectedKey = $"{ExpectedAllCountriesKey}:{country.Cca2}";

        _repository.Save(country);

        _cache.Received(1).Save(expectedKey, country);
    }

    [Test]
    public async Task GetAllAsyncShouldGetAllCountries()
    {
        await _repository.GetAllAsync(CancellationToken.None);

        await _dataSource.Received(1).QueryAllAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAllAsyncShouldExcludedCountries()
    {
        Country canada = CreateCountry("CA");
        Country italy = CreateCountry("IT");

        _dataSource.QueryAllAsync(Arg.Any<CancellationToken>()).Returns([canada, italy]);

        Country[] countries = await _repository.GetAllAsync(CancellationToken.None);

        await Assert.That(countries).Contains(canada);
        await Assert.That(countries).DoesNotContain(italy);
    }

    [Test]
    public async Task GetAllAsyncShouldCacheAllCountries()
    {
        Country country = CreateCountry("CA");
        _dataSource.QueryAllAsync(Arg.Any<CancellationToken>()).Returns([country]);

        await _repository.GetAllAsync(CancellationToken.None);

        _cache.Received(1).Save(ExpectedAllCountriesKey, Arg.Is<Country[]>(c => c.Contains(country)));
    }

    [Test]
    public async Task GetAllAsyncShouldNotRetrieveFromDataSourceIsAllCountriesAreCached()
    {
        _cache.TryGet<Country[]>(ExpectedAllCountriesKey, out _).Returns(returnThis: true);

        await _repository.GetAllAsync(CancellationToken.None);

        await _dataSource.DidNotReceive().QueryAllAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetAsyncShouldGetAllCountries()
    {
        await _repository.GetAsync(new Cca2("CA"), CancellationToken.None);

        await _dataSource.Received(1).QueryAllAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAsyncShouldGetCountryByCca2()
    {
        Country canada = CreateCountry("CA");
        Country usa = CreateCountry("US");

        _dataSource.QueryAllAsync(CancellationToken.None).Returns([canada, usa]);

        Country? foundCountry = await _repository.GetAsync(canada.Cca2, CancellationToken.None);

        await Assert.That(foundCountry).IsEqualTo(canada);
    }

    [Test]
    public async Task GetAsyncShouldReturnNullWhenDoesNotFoundTheCountry()
    {
        Country country = CreateCountry("CA");
        _dataSource.QueryAllAsync(CancellationToken.None).Returns([country]);

        Country? foundCountry = await _repository.GetAsync(new Cca2("US"), CancellationToken.None);

        await Assert.That(foundCountry).IsNull();
    }

    [Test]
    public async Task GetAsyncShouldCacheTheCountry()
    {
        Country country = CreateCountry("CA");
        _dataSource.QueryAllAsync(CancellationToken.None).Returns([country]);

        string expectedKey = $"{ExpectedAllCountriesKey}:{country.Cca2}";

        await _repository.GetAsync(country.Cca2, CancellationToken.None);

        _cache.Received(1).Save(expectedKey, Arg.Is<Country>(c => c.Cca2 == country.Cca2));
    }

    [Test]
    public async Task GetAsyncShouldNotRetrieveFromDataSourceWhenCountryIsCached()
    {
        Cca2 cca2 = new("CA");
        string expectedKey = $"{ExpectedAllCountriesKey}:{cca2}";

        _cache.TryGet<Country>(expectedKey, out _).Returns(returnThis: true);

        await _repository.GetAsync(cca2, CancellationToken.None);

        await _dataSource.DidNotReceive().QueryAllAsync(CancellationToken.None);
    }

    private static Country CreateCountry(string cca2) => new()
    {
        Cca2 = new Cca2(cca2),
        Capitals = [new Capital("Ottawa", new Coordinate(0, 0))],
        Area = new Area(1),
        Population = 1,
        Borders = [new Cca2("US")],
        Continent = Continent.NorthAmerica,
        Coordinate = new Coordinate(0, 0),
        Resources = new Resources(new Uri("https://www.google.com/maps/place/Canada"), new Uri("https://canada.svg"), new Uri("https://canada.coat-of-arms.svg"))
    };
}
