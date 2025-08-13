// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Countries;
using Infrastructure.Persistence.Countries.Options;
using Infrastructure.Persistence.Countries.Sources;

namespace Unit.Tests.Infrastructure.Persistence.Countries;

internal sealed class CountryLookupRepositoryTests
{
    private const string ExpectedAllCountriesKey = "countries:lookup";

    private readonly IDataSource<CountryLookup> _dataSource = Substitute.For<IDataSource<CountryLookup>>();
    private readonly ICache _cache = Substitute.For<ICache>();

    private readonly CountryLookupRepository _repository;

    public CountryLookupRepositoryTests()
    {
        _repository = new CountryLookupRepository(_dataSource, _cache, new ExcludedCountriesOptions()
        {
            Countries = ["CA"]
        });
    }

    [Test]
    public async Task LookupAsyncShouldGetAllCountries()
    {
        await _repository.LookupAsync(CancellationToken.None);

        await _dataSource.Received(1).QueryAllAsync(CancellationToken.None);
    }

    [Test]
    public async Task LookupAsyncShouldExcludeCountries()
    {
        CountryLookup canada = CreateCountry("CA");
        CountryLookup italy = CreateCountry("IT");

        _dataSource.QueryAllAsync(Arg.Any<CancellationToken>()).Returns([canada, italy]);

        CountryLookup[] countries = await _repository.LookupAsync(CancellationToken.None);

        await Assert.That(countries).DoesNotContain(canada);
        await Assert.That(countries).Contains(italy);
    }

    [Test]
    public async Task LookupAsyncShouldCacheAllCountries()
    {
        CountryLookup country = CreateCountry("CA");
        _dataSource.QueryAllAsync(Arg.Any<CancellationToken>()).Returns([country]);

        await _repository.LookupAsync(CancellationToken.None);

        _cache.Received(1).Save(ExpectedAllCountriesKey, Arg.Is<CountryLookup[]>(c => c.Contains(country)));
    }

    [Test]
    public async Task LookupAsyncShouldNotRetrieveFromDataSourceWhenAllCountriesAreCached()
    {
        _cache.TryGet<CountryLookup[]>(ExpectedAllCountriesKey, out _).Returns(returnThis: true);

        await _repository.LookupAsync(CancellationToken.None);

        await _dataSource.DidNotReceive().QueryAllAsync(Arg.Any<CancellationToken>());
    }

    private static CountryLookup CreateCountry(string cca2) => new()
    {
        Cca2 = new Cca2(cca2),
        Translations = [new Translation(Language.English, "Canada")]
    };
}
