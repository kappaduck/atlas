// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Infrastructure.Persistence.Changelog;
using System.Net;
using TUnit.Mocks.Http;

namespace Unit.Tests.Infrastructure.Changelog;

public sealed class ChangelogClientTests
{
    private const string Content = "content";

    private readonly MockHttpClient _http = Mock.HttpClient("https://test.atlas.com");
    private readonly ChangelogClient _client;
    private readonly ChangelogEndpointOptions _options = new()
    {
        Url = "/changelog"
    };

    public ChangelogClientTests()
    {
        _http.Handler.OnGet(_options.Url).RespondWithJson(Content);
        _client = new ChangelogClient(_http, _options);
    }

    [Test]
    public async Task GetAsyncShouldCallChangelog()
    {
        await _client.GetAsync(CancellationToken.None);
        _http.Handler.Verify(r => r.Method(HttpMethod.Get).Path(_options.Url), Times.Once);
    }

    [Test]
    public async Task GetAsyncShouldGiveNullWhenStatusCodeIsNotOK200()
    {
        _http.Handler.OnGet(_options.Url).Respond(HttpStatusCode.InternalServerError);

        string? content = await _client.GetAsync(CancellationToken.None);
        await Assert.That(content).IsNull();
    }

    [Test]
    public async Task GetAsyncShouldGiveContentWhenStatusCodeIsOK200()
    {
        string? content = await _client.GetAsync(CancellationToken.None);
        await Assert.That(content).IsEqualTo(Content);
    }

    [Test]
    public async Task GetAsyncShouldGiveNullWhenThereIsNoContent()
    {
        _http.Handler.OnGet(_options.Url).RespondWith(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(string.Empty)
        });

        string? content = await _client.GetAsync(CancellationToken.None);
        await Assert.That(content).IsNull();
    }
}
