// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Countries;
using Unit.Tests.Data;

namespace Unit.Tests.Infrastructure.Countries;

[ClassDataSource<CountryData>]
public class CountryRepositoryTests
{
    private const string ExpectedCountriesKey = "countries";
    private const string ExpectedLookupKey = "countries:lookup";

    private readonly CountryData _countries;
    private readonly ICountryClient _client = Substitute.For<ICountryClient>();
    private readonly ICache _cache = Substitute.For<ICache>();
    private readonly CountryRepository _repository;

    public CountryRepositoryTests(CountryData countries)
    {
        _countries = countries;

        _client.GetAsync(CancellationToken.None).Returns([countries.Canada]);
        _client.LookupAsync(CancellationToken.None).Returns([countries.Canada.Cca2]);

        _repository = new CountryRepository(_client, _cache);
    }

    [Test]
    public void SaveShouldCacheCountry()
    {
        string expectedKey = $"{ExpectedCountriesKey}:{_countries.Canada.Cca2}";

        _repository.Save(_countries.Canada);
        _cache.Received(1).Save(expectedKey, _countries.Canada);
    }

    [Test]
    public async Task GetAllAsyncShouldGetAllCountries()
    {
        await _repository.GetAllAsync(CancellationToken.None);
        await _client.Received(1).GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAllAsyncShouldCacheAllCountries()
    {
        await _repository.GetAllAsync(CancellationToken.None);
        _cache.Received(1).Save(ExpectedCountriesKey, NSubstitute.Arg.Is<IEnumerable<Country>>(c => c!.Contains(_countries.Canada)));
    }

    [Test]
    public async Task GetAllAsyncShouldNotCallTheClientWhenCountriesAreCached()
    {
        _cache.TryGet<IEnumerable<Country>>(ExpectedCountriesKey, out _).Returns(returnThis: true);

        await _repository.GetAllAsync(CancellationToken.None);
        await _client.DidNotReceive().GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAllAsyncShouldGetCachedCountriesWhenAreCached()
    {
        _cache.TryGet(ExpectedCountriesKey, out NSubstitute.Arg.Any<IEnumerable<Country>?>()).Returns(x =>
        {
            x[1] = new Country[] { _countries.Canada };
            return true;
        });

        IEnumerable<Country> countries = await _repository.GetAllAsync(CancellationToken.None);
        await Assert.That(countries.First()).IsEqualTo(_countries.Canada);
    }

    [Test]
    public async Task GetAsyncShouldGetAllCountries()
    {
        await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        await _client.Received(1).GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAsyncShouldCacheAllCountries()
    {
        await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        _cache.Received(1).Save(ExpectedCountriesKey, NSubstitute.Arg.Is<IEnumerable<Country>>(c => c!.Contains(_countries.Canada)));
    }

    [Test]
    public async Task GetAsyncShouldNotCallTheClientWhenCountriesAreCached()
    {
        _cache.TryGet<IEnumerable<Country>>(ExpectedCountriesKey, out _).Returns(x =>
        {
            x[1] = new Country[] { _countries.Canada };
            return true;
        });

        await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        await _client.DidNotReceive().GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAsyncShouldGetCachedCountriesWhenAreCached()
    {
        _cache.TryGet(ExpectedCountriesKey, out NSubstitute.Arg.Any<IEnumerable<Country>?>()).Returns(x =>
        {
            x[1] = new Country[] { _countries.Canada };
            return true;
        });

        Country? country = await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        await Assert.That(country).IsEqualTo(_countries.Canada);
    }

    [Test]
    public async Task GetAsyncShouldGetCountryByCca2()
    {
        Country? foundCountry = await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        await Assert.That(foundCountry).IsEqualTo(_countries.Canada);
    }

    [Test]
    public async Task GetAsyncShouldReturnNullWhenDoesNotFoundTheCountry()
    {
        Country? foundCountry = await _repository.GetAsync(new Cca2("US"), CancellationToken.None);
        await Assert.That(foundCountry).IsNull();
    }

    [Test]
    public async Task GetAsyncShouldNotCacheCountryWhenIsNull()
    {
        const string key = $"{ExpectedCountriesKey}:IT";

        await _repository.GetAsync(new Cca2("IT"), CancellationToken.None);
        _cache.DidNotReceive().Save(key, NSubstitute.Arg.Any<Country>());
    }

    [Test]
    public async Task GetAsyncShouldCacheTheCountry()
    {
        string expectedKey = $"{ExpectedCountriesKey}:{_countries.Canada.Cca2}";

        await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        _cache.Received(1).Save(expectedKey, _countries.Canada);
    }

    [Test]
    public async Task GetAsyncShouldNotGetFromClientWhenCountryIsCached()
    {
        string expectedKey = $"{ExpectedCountriesKey}:{_countries.Canada.Cca2}";
        _cache.TryGet<Country>(expectedKey, out _).Returns(returnThis: true);

        await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        await _client.DidNotReceive().GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAsyncShouldGetCachedCountryWhenIsCached()
    {
        string expectedKey = $"{ExpectedCountriesKey}:{_countries.Canada.Cca2}";
        _cache.TryGet<Country>(expectedKey, out _).Returns(x =>
        {
            x[1] = _countries.Canada;
            return true;
        });

        Country? country = await _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None);
        await Assert.That(country).IsEqualTo(_countries.Canada);
    }

    [Test]
    public async Task LookupAsyncShouldGetLookupCountries()
    {
        await _repository.LookupAsync(CancellationToken.None);
        await _client.Received(1).LookupAsync(CancellationToken.None);
    }

    [Test]
    public async Task LookupAsyncShouldCacheAllCountries()
    {
        await _repository.LookupAsync(CancellationToken.None);
        _cache.Received(1).Save(ExpectedLookupKey, NSubstitute.Arg.Is<IEnumerable<Cca2>>(c => c!.Contains(_countries.Canada.Cca2)));
    }

    [Test]
    public async Task LookupAsyncShouldNotCallTheClientWhenCountriesAreCached()
    {
        _cache.TryGet<IEnumerable<Cca2>>(ExpectedLookupKey, out _).Returns(returnThis: true);

        await _repository.LookupAsync(CancellationToken.None);
        await _client.DidNotReceive().LookupAsync(CancellationToken.None);
    }

    [Test]
    public async Task LookupAsyncShouldGetCachedCountriesWhenAreCached()
    {
        _cache.TryGet(ExpectedLookupKey, out NSubstitute.Arg.Any<IEnumerable<Cca2>?>()).Returns(x =>
        {
            x[1] = new Cca2[] { _countries.Canada.Cca2 };
            return true;
        });

        IEnumerable<Cca2> countries = await _repository.LookupAsync(CancellationToken.None);
        await Assert.That(countries.First()).IsEqualTo(_countries.Canada.Cca2);
    }
}
