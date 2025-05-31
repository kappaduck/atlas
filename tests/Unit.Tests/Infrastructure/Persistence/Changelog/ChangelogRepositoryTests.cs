// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Changelog;
using Infrastructure.Persistence.Changelog.Sources;

namespace Unit.Tests.Infrastructure.Persistence.Changelog;

internal sealed class ChangelogRepositoryTests
{
    private const string ExpectedKey = "changelog";

    private readonly IChangelogClient _client = Substitute.For<IChangelogClient>();
    private readonly ICache _cache = Substitute.For<ICache>();

    private readonly ChangelogRepository _repository;

    public ChangelogRepositoryTests()
    {
        _repository = new ChangelogRepository(_client, _cache);
    }

    [Test]
    public async Task GetAsyncShouldGetTheChangelog()
    {
        await _repository.GetAsync(CancellationToken.None);

        await _client.Received(1).GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAsyncShouldReturnNullWhenDoesNotFoundTheChangelog()
    {
        const string? changelog = null;
        _client.GetAsync(CancellationToken.None).Returns(changelog);

        string? result = await _repository.GetAsync(CancellationToken.None);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task GetAsyncShouldCacheTheChangelog()
    {
        const string changelog = "Changelog content";
        _client.GetAsync(CancellationToken.None).Returns(changelog);

        await _repository.GetAsync(CancellationToken.None);

        _cache.Received(1).Save(ExpectedKey, changelog);
    }

    [Test]
    public async Task GetAsyncShouldNotRetrieveFromClientWhenChangelogIsCached()
    {
        _cache.TryGet<string>(ExpectedKey, out _).Returns(returnThis: true);

        await _repository.GetAsync(CancellationToken.None);

        await _client.DidNotReceive().GetAsync(CancellationToken.None);
    }
}
