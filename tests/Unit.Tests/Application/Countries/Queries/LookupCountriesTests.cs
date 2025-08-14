// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;
using Unit.Tests.Fixtures;

namespace Unit.Tests.Application.Countries.Queries;

[ClassDataSource<LocalizerFixture>]
public sealed class LookupCountriesTests
{
    private readonly LocalizerFixture _localizer;
    private readonly ICountryLookupRepository _repository = Substitute.For<ICountryLookupRepository>();

    private readonly LookupCountries.Query _query = new();
    private readonly LookupCountries.Handler _handler;

    public LookupCountriesTests(LocalizerFixture localizer)
    {
        _localizer = localizer;
        _handler = new LookupCountries.Handler(_repository, _localizer.Countries);
    }

    [Test]
    public async Task HandleShouldGetCountriesFromRepository()
    {
        _repository.LookupAsync(CancellationToken.None).Returns([]);

        await _handler.Handle(_query, CancellationToken.None);

        await _repository.Received(1).LookupAsync(CancellationToken.None);
    }

    [Test]
    public async Task HandleShouldReturnCountries()
    {
        Cca2 cca2 = new("CA");
        _localizer.Countries[cca2].Returns(new LocalizedString(cca2, "Canada"));

        _repository.LookupAsync(CancellationToken.None).Returns([cca2]);

        CountryLookupResponse[] response = await _handler.Handle(_query, CancellationToken.None);

        await Assert.That(response[0].Cca2).IsEqualTo(cca2);
        await Assert.That(response[0].Name).IsEqualTo("Canada");
    }
}
