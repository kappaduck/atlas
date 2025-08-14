// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Countries;
using Infrastructure.Persistence.Countries.Options;
using Infrastructure.Persistence.Countries.Sources;

namespace Unit.Tests.Infrastructure.Persistence.Countries;

public sealed class CountryLookupRepositoryTests
{
    private const string ExpectedAllCountriesKey = "countries:lookup";

    private readonly IDataSource<Cca2> _dataSource = Substitute.For<IDataSource<Cca2>>();
    private readonly ICache _cache = Substitute.For<ICache>();

    private readonly CountryLookupRepository _repository;

    public CountryLookupRepositoryTests()
    {
        _repository = new CountryLookupRepository(_dataSource, _cache, new ExcludedCountriesOptions()
        {
            Excluded = ["IT"]
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
        Cca2 canada = new("CA");
        Cca2 italy = new("IT");

        _dataSource.QueryAllAsync(Arg.Any<CancellationToken>()).Returns([canada, italy]);

        Cca2[] countries = await _repository.LookupAsync(CancellationToken.None);

        await Assert.That(countries).Contains(canada);
        await Assert.That(countries).DoesNotContain(italy);
    }

    [Test]
    public async Task LookupAsyncShouldCacheAllCountries()
    {
        Cca2 country = new("CA");
        _dataSource.QueryAllAsync(Arg.Any<CancellationToken>()).Returns([country]);

        await _repository.LookupAsync(CancellationToken.None);

        _cache.Received(1).Save(ExpectedAllCountriesKey, Arg.Is<Cca2[]>(c => c.Contains(country)));
    }

    [Test]
    public async Task LookupAsyncShouldNotRetrieveFromDataSourceWhenAllCountriesAreCached()
    {
        _cache.TryGet<Cca2[]>(ExpectedAllCountriesKey, out _).Returns(returnThis: true);

        await _repository.LookupAsync(CancellationToken.None);

        await _dataSource.DidNotReceive().QueryAllAsync(Arg.Any<CancellationToken>());
    }
}
