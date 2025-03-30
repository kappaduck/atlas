// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Logging;
using MockHttp;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Options;
using Prometheus.Countries.Providers;
using System.Net;
using Unit.Tests.Fixtures;

namespace Unit.Tests.Prometheus.Countries.Providers;

internal sealed class CountryEndpointTests : IDisposable
{
    private const string EndpointUrl = "https://test.countries.com";

    private readonly MockHttpHandler _handler = new();
    private readonly HttpClient _client;

    private readonly CountryEndpoint _endpoint;

    public CountryEndpointTests()
    {
        CountryEndpointOptions options = new() { Endpoint = EndpointUrl };
        ILogger<CountryEndpoint> logger = Substitute.For<ILogger<CountryEndpoint>>();

        _client = new HttpClient(_handler);

        _endpoint = new CountryEndpoint(_client, logger, options);
    }

    public void Dispose()
    {
        _handler.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task GetAllAsyncShouldFetchFromEndpoint()
    {
        const string body = "[]";

        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(body));

        await _endpoint.GetAllAsync(CancellationToken.None);

        await _handler.VerifyAsync(h => h.Method(HttpMethod.Get).RequestUri(EndpointUrl), IsSent.Once);
    }

    [Test]
    public async Task GetAllAsyncShouldReturnEmptyWhenStatusIsNot200()
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.NotFound));

        CountryDto[] countries = await _endpoint.GetAllAsync(CancellationToken.None);

        await Assert.That(countries).IsEmpty();
    }

    [Test]
    [ClassDataSource<CanadaDtoJson>]
    public async Task GetAllAsyncShouldReturnCountries(CanadaDtoJson json)
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(json.Value));

        CountryDto[] countries = await _endpoint.GetAllAsync(CancellationToken.None);

        CountryDto canada = countries[0];

        using (Assert.Multiple())
        {
            await Assert.That(canada.Cca2).IsEqualTo("CA");
            await Assert.That(canada.Name.Common).IsEqualTo("Canada");
            await Assert.That(canada.Capitals).Contains("Ottawa");
            await Assert.That(canada.Region).IsEqualTo(RegionDto.Americas);
            await Assert.That(canada.SubRegion).IsEqualTo(SubRegionDto.NorthAmerica);
            await Assert.That(canada.Translations).Contains(new TranslationDto("fra", "Canada"));
            await Assert.That(canada.Coordinate).IsEqualTo(new CoordinateDto(60.0, -95.0));
            await Assert.That(canada.Borders).Contains("USA");
            await Assert.That(canada.Area).IsEqualTo(9984670);
            await Assert.That(canada.Population).IsEqualTo(38005238);
            await Assert.That(canada.CapitalInfo.Coordinate).IsEqualTo(new CoordinateDto(45.42, -75.7));
        }
    }

    [Test]
    [ClassDataSource<AntarcticaDtoJson>]
    public async Task GetAllAsyncShouldReturnAllPossibleValuesFromACountryWhichMissingSomeFields(AntarcticaDtoJson json)
    {
        _handler.When(h => h.RequestUri(EndpointUrl))
                .Respond(h => h.StatusCode(HttpStatusCode.OK).Body(json.Value));

        CountryDto[] countries = await _endpoint.GetAllAsync(CancellationToken.None);

        CountryDto antarctica = countries[0];

        using (Assert.Multiple())
        {
            await Assert.That(antarctica.Cca2).IsEqualTo("AQ");
            await Assert.That(antarctica.Name.Common).IsEqualTo("Antarctica");
            await Assert.That(antarctica.Capitals).IsNull();
            await Assert.That(antarctica.Region).IsEqualTo(RegionDto.Antarctic);
            await Assert.That(antarctica.SubRegion).IsNull();
            await Assert.That(antarctica.Translations).Contains(new TranslationDto("fra", "Antarctique"));
            await Assert.That(antarctica.Coordinate).IsEqualTo(new CoordinateDto(-90.0, 0.0));
            await Assert.That(antarctica.Borders).IsNull();
            await Assert.That(antarctica.Area).IsEqualTo(14000000);
            await Assert.That(antarctica.Population).IsEqualTo(1000);
            await Assert.That(antarctica.CapitalInfo.Coordinate).IsNull();
        }
    }
}
