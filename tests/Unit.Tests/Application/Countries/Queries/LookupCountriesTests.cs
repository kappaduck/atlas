// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Domain.Languages;

namespace Unit.Tests.Application.Countries.Queries;

public sealed class LookupCountriesTests
{
    private readonly ICountryLookupRepository _repository = Substitute.For<ICountryLookupRepository>();

    private readonly LookupCountries.Query _query = new();
    private readonly LookupCountries.Handler _handler;

    public LookupCountriesTests()
    {
        _handler = new LookupCountries.Handler(_repository);
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
        CountryLookup country = new()
        {
            Cca2 = new Cca2("CA"),
            Translations = [new Translation(Language.English, "Canada")]
        };

        _repository.LookupAsync(CancellationToken.None).Returns([country]);

        CountryLookupResponse[] response = await _handler.Handle(_query, CancellationToken.None);

        await Assert.That(response[0].Cca2).IsEqualTo(country.Cca2);
        await Assert.That(response[0].Name).IsEqualTo("Canada");
    }
}
