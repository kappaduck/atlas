// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.Extensions.Localization;
using Unit.Tests.Data;
using Unit.Tests.Mocks;

namespace Unit.Tests.Application.Countries.Services;

[ClassDataSource(typeof(CountryData), typeof(LocalizerMock))]
public sealed class CountryServiceTests
{
    private readonly CountryData _countries;
    private readonly Mock<ICountryRepository> _repository = ICountryRepository.Mock();

    private readonly CountryService _service;

    public CountryServiceTests(CountryData countries, LocalizerMock localizer)
    {
        _countries = countries;

        _repository.GetAllAsync(CancellationToken.None).Returns([countries.Canada]);
        _repository.GetAsync(countries.Italy.Cca2, CancellationToken.None).Returns(countries.Italy);

        localizer.Application[countries.Canada.Cca2].Returns(new LocalizedString(countries.Canada.Cca2, "Canada"));
        localizer.Application[countries.Italy.Cca2].Returns(new LocalizedString(countries.Italy.Cca2, "Italy"));
        localizer.Application[countries.Canada.Continent.ToString()].Returns(new LocalizedString(countries.Canada.Continent.ToString(), "North America"));
        localizer.Application[countries.Italy.Continent.ToString()].Returns(new LocalizedString(countries.Italy.Continent.ToString(), "Europe"));

        _service = new CountryService(_repository.Object, localizer.Application);
    }

    [Test]
    public async Task GetAllAsyncShouldGetAllCountries()
    {
        await _service.GetAllAsync(CancellationToken.None);
        _repository.GetAllAsync(CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task GetAllAsyncShouldReturnCountries()
    {
        CountryListItem[] countries = [.. await _service.GetAllAsync(CancellationToken.None)];
        CountryListItem country = countries[0];

        await Assert.That(country.Name).IsEqualTo("Canada");
    }

    [Test]
    public async Task GetAsyncShouldGetCountry()
    {
        await _service.GetAsync(_countries.Italy.Cca2, CancellationToken.None);
        _repository.GetAsync(_countries.Italy.Cca2, CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task GetAsyncShouldReturnTheCountry()
    {
        CountryResponse country = await _service.GetAsync(_countries.Italy.Cca2, CancellationToken.None);
        await Assert.That(country.Cca2).IsEqualTo(_countries.Italy.Cca2);
    }

    [Test]
    public async Task GetDailyCountryAsyncShouldGetAllCountries()
    {
        await _service.GetDailyCountryAsync(CancellationToken.None);
        _repository.GetAllAsync(CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task GetDailyCountryAsyncShouldReturnTheDailyCountry()
    {
        CountryResponse country = await _service.GetDailyCountryAsync(CancellationToken.None);
        await Assert.That(country.Cca2).IsEqualTo(_countries.Canada.Cca2);
    }

    [Test]
    public async Task GetDailyCountryAsyncShouldSaveTheDailyCountry()
    {
        await _service.GetDailyCountryAsync(CancellationToken.None);
        _repository.Save(_countries.Canada).WasCalled(Times.Once);
    }

    [Test]
    public async Task GetDailyFlagAsyncShouldGetAllCountries()
    {
        await _service.GetDailyFlagAsync(CancellationToken.None);
        _repository.GetAllAsync(CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task GetDailyFlagAsyncShouldReturnTheDailyCountry()
    {
        CountryResponse country = await _service.GetDailyFlagAsync(CancellationToken.None);
        await Assert.That(country.Cca2).IsEqualTo(_countries.Canada.Cca2);
    }

    [Test]
    public async Task GetDailyFlagAsyncShouldSaveTheDailyCountry()
    {
        await _service.GetDailyFlagAsync(CancellationToken.None);
        _repository.Save(_countries.Canada).WasCalled(Times.Once);
    }

    [Test]
    public async Task GuessAsyncShouldGetTheGuessedCountry()
    {
        _repository.GetAsync(_countries.Italy.Cca2, CancellationToken.None).Returns(_countries.Italy);
        _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None).Returns(_countries.Canada);

        await _service.GuessAsync(_countries.Italy.Cca2, _countries.Canada.Cca2, CancellationToken.None);
        _repository.GetAsync(_countries.Italy.Cca2, CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task GuessAsyncShouldGetTheCountry()
    {
        _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None).Returns(_countries.Canada);

        await _service.GuessAsync(_countries.Italy.Cca2, _countries.Canada.Cca2, CancellationToken.None);
        _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task GuessAsyncShouldReturnBadGuessedCountryWhenIsNotSameCountry()
    {
        _repository.GetAsync(_countries.Canada.Cca2, CancellationToken.None).Returns(_countries.Canada);

        GuessedResponse guess = await _service.GuessAsync(_countries.Canada.Cca2, _countries.Italy.Cca2, CancellationToken.None);

        WrongGuessResponse wrong = (WrongGuessResponse)guess.Value;

        await Assert.That(wrong.Cca2).IsEqualTo(_countries.Canada.Cca2);
        await Assert.That(wrong.Name).IsEqualTo("Canada");
        await Assert.That(wrong.Continent).IsEqualTo("North America");
        await Assert.That(wrong.IsSameContinent).IsFalse();
        await Assert.That(wrong.Direction).IsEqualTo(104);
        await Assert.That(wrong.Kilometers).IsEqualTo(6843);
        await Assert.That(wrong.Miles).IsEqualTo(4252);
        await Assert.That(wrong.Proximity).IsEqualTo(66);
        await Assert.That(wrong.Flag).IsEqualTo(_countries.Canada.Resources.Flag);
    }

    [Test]
    public async Task GuessAsyncShouldReturnGoodGuessedCountryWhenIsNotSameCountry()
    {
        GuessedResponse guess = await _service.GuessAsync(_countries.Italy.Cca2, _countries.Italy.Cca2, CancellationToken.None);

        GoodGuessResponse good = (GoodGuessResponse)guess.Value;

        await Assert.That(good.Cca2).IsEqualTo(_countries.Italy.Cca2);
        await Assert.That(good.Name).IsEqualTo("Italy");
        await Assert.That(good.Continent).IsEqualTo("Europe");
        await Assert.That(good.Flag).IsEqualTo(_countries.Italy.Resources.Flag);
        await Assert.That(good.Map).IsEqualTo(_countries.Italy.Resources.Map);
    }

    [Test]
    public async Task RandomizeAsyncShouldGetAllCountries()
    {
        await _service.RandomizeAsync(CancellationToken.None);
        _repository.GetAllAsync(CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task RandomizeAsyncShouldReturnTheRandomizedCountry()
    {
        CountryResponse country = await _service.RandomizeAsync(CancellationToken.None);
        await Assert.That(country.Cca2).IsEqualTo(_countries.Canada.Cca2);
    }

    [Test]
    public async Task RandomizeAsyncShouldSaveTheRandomizedCountry()
    {
        await _service.RandomizeAsync(CancellationToken.None);
        _repository.Save(_countries.Canada).WasCalled(Times.Once);
    }
}
