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
        CountryResponse[] countries = [.. await _service.GetAllAsync(CancellationToken.None)];
        CountryResponse country = countries[0];

        await Assert.That(country.Cca2).IsEqualTo(_countries.Canada.Cca2);
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
        CountryResponse? country = await _service.GetAsync(_countries.Italy.Cca2, CancellationToken.None);
        await Assert.That(country!.Cca2).IsEqualTo(_countries.Italy.Cca2);
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
        CountryResponse? country = await _service.GetDailyCountryAsync(CancellationToken.None);
        await Assert.That(country!.Cca2).IsEqualTo(_countries.Canada.Cca2);
    }

    [Test]
    public async Task GetDailyCountryAsyncShouldReturnNullWhenThereIsNoCountries()
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([]);

        CountryResponse? country = await _service.GetDailyCountryAsync(CancellationToken.None);
        await Assert.That(country).IsNull();
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
        CountryResponse? country = await _service.GetDailyFlagAsync(CancellationToken.None);
        await Assert.That(country!.Cca2).IsEqualTo(_countries.Canada.Cca2);
    }

    [Test]
    public async Task GetDailyFlagAsyncShouldReturnNullWhenThereIsNoCountries()
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([]);

        CountryResponse? country = await _service.GetDailyFlagAsync(CancellationToken.None);
        await Assert.That(country).IsNull();
    }

    [Test]
    public async Task GetDailyFlagAsyncShouldSaveTheDailyCountry()
    {
        await _service.GetDailyFlagAsync(CancellationToken.None);
        _repository.Save(_countries.Canada).WasCalled(Times.Once);
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
        CountryResponse? country = await _service.RandomizeAsync(CancellationToken.None);
        await Assert.That(country!.Cca2).IsEqualTo(_countries.Canada.Cca2);
    }

    [Test]
    public async Task RandomizeAsyncShouldReturnNullWhenThereIsNoCountries()
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([]);

        CountryResponse? country = await _service.RandomizeAsync(CancellationToken.None);
        await Assert.That(country).IsNull();
    }

    [Test]
    public async Task RandomizeAsyncShouldSaveTheRandomizedCountry()
    {
        await _service.RandomizeAsync(CancellationToken.None);
        _repository.Save(_countries.Canada).WasCalled(Times.Once);
    }
}
