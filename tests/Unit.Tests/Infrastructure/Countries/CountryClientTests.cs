// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries;
using System.Net;
using System.Net.Http.Json;
using TUnit.Mocks.Http;
using Unit.Tests.Data;

namespace Unit.Tests.Infrastructure.Countries;

[ClassDataSource<CountryData>]
public sealed class CountryClientTests
{
    private readonly MockHttpClient _http = Mock.HttpClient("https://test.atlas.com");
    private readonly CountryClient _client;
    private readonly CountryEndpointOptions _options = new()
    {
        All = "/all",
        Lookup = "/lookup"
    };

    public CountryClientTests(CountryData data)
    {
        _http.Handler.OnGet(_options.All).RespondWithJson(data.All);
        _http.Handler.OnGet(_options.Lookup).RespondWithJson(data.Lookup);

        _client = new CountryClient(_http, _options);
    }

    [Test]
    public async Task GetAsyncShouldCallAll()
    {
        await _client.GetAsync(CancellationToken.None);
        _http.Handler.Verify(r => r.Method(HttpMethod.Get).Path(_options.All), Times.Once);
    }

    [Test]
    public async Task GetAsyncShouldGiveEmptyWhenStatusCodeIsNotOK200()
    {
        _http.Handler.OnGet(_options.All).Respond(HttpStatusCode.InternalServerError);

        IEnumerable<Country> countries = await _client.GetAsync(CancellationToken.None);
        await Assert.That(countries).IsEmpty();
    }

    [Test]
    public async Task GetAsyncShouldGiveCountriesWhenStatusCodeIsOK200()
    {
        IEnumerable<Country> countries = await _client.GetAsync(CancellationToken.None);
        await Assert.That(countries).IsNotEmpty();
    }

    [Test]
    public async Task GetAsyncShouldGiveEmptyWhenThereIsNoContent()
    {
        _http.Handler.OnGet(_options.All).RespondWith(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create<Country[]?>(null)
        });

        IEnumerable<Country> countries = await _client.GetAsync(CancellationToken.None);
        await Assert.That(countries).IsEmpty();
    }

    [Test]
    public async Task LookupAsyncShouldCallLookup()
    {
        await _client.LookupAsync(CancellationToken.None);
        _http.Handler.Verify(r => r.Method(HttpMethod.Get).Path(_options.Lookup), Times.Once);
    }

    [Test]
    public async Task LookupAsyncShouldGiveEmptyWhenStatusCodeIsNotOK200()
    {
        _http.Handler.OnGet(_options.Lookup).Respond(HttpStatusCode.InternalServerError);

        IEnumerable<Cca2> codes = await _client.LookupAsync(CancellationToken.None);
        await Assert.That(codes).IsEmpty();
    }

    [Test]
    public async Task LookupAsyncShouldGiveCountriesWhenStatusCodeIsOK200()
    {
        IEnumerable<Cca2> codes = await _client.LookupAsync(CancellationToken.None);
        await Assert.That(codes).IsNotEmpty();
    }

    [Test]
    public async Task LookupAsyncShouldGiveEmptyWhenThereIsNoContent()
    {
        _http.Handler.OnGet(_options.Lookup).RespondWith(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create<Cca2[]?>(null)
        });

        IEnumerable<Cca2> codes = await _client.LookupAsync(CancellationToken.None);
        await Assert.That(codes).IsEmpty();
    }
}
