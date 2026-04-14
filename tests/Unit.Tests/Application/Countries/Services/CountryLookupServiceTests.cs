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
public sealed class CountryLookupServiceTests
{
    private readonly CountryData _countries;
    private readonly Mock<ICountryRepository> _repository = ICountryRepository.Mock();

    private readonly CountryLookupService _service;

    public CountryLookupServiceTests(CountryData countries, LocalizerMock localizer)
    {
        _countries = countries;

        _repository.LookupAsync(CancellationToken.None).Returns([countries.Canada.Cca2]);
        localizer.Application[countries.Canada.Cca2].Returns(new LocalizedString(countries.Canada.Cca2, "Canada"));

        _service = new CountryLookupService(_repository.Object, localizer.Application);
    }

    [Test]
    public async Task LookupAsyncShouldCallRepository()
    {
        await _service.LookupAsync(CancellationToken.None);
        _repository.LookupAsync(CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task LookupAsyncShouldGetCountries()
    {
        CountryLookupResponse[] countries = [.. await _service.LookupAsync(CancellationToken.None)];
        CountryLookupResponse country = countries[0];

        await Assert.That(country.Cca2).IsEqualTo(_countries.Canada.Cca2);
        await Assert.That(country.Name).IsEqualTo("Canada");
    }
}
