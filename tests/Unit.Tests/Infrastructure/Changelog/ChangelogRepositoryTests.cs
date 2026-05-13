// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Changelog;

namespace Unit.Tests.Infrastructure.Changelog;

public sealed class ChangelogRepositoryTests
{
    private const string ExpectedKey = "changelog";
    private const string Content = "changelog content";

    private readonly IChangelogClient _client = Substitute.For<IChangelogClient>();
    private readonly ICache _cache = Substitute.For<ICache>();

    private readonly ChangelogRepository _repository;

    public ChangelogRepositoryTests()
    {
        _client.GetAsync(CancellationToken.None).Returns(Content);
        _repository = new ChangelogRepository(_client, _cache);
    }

    [Test]
    public async Task GetAsyncShouldGetTheChangelog()
    {
        await _repository.GetAsync(CancellationToken.None);
        await _client.Received(1).GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAsyncShouldCacheTheChangelog()
    {
        await _repository.GetAsync(CancellationToken.None);
        _cache.Received(1).Save(ExpectedKey, Content);
    }

    [Test]
    public async Task GetAsyncShouldNotCallTheClientWhenChangelogIsCached()
    {
        _cache.TryGet<string>(ExpectedKey, out _).Returns(returnThis: true);

        await _repository.GetAsync(CancellationToken.None);
        await _client.DidNotReceive().GetAsync(CancellationToken.None);
    }

    [Test]
    public async Task GetAsyncShouldGetCachedChangelogWhenIsCached()
    {
        _cache.TryGet(ExpectedKey, out NSubstitute.Arg.Is<string?>(Content)).Returns(x =>
        {
            x[1] = Content;
            return true;
        });

        string? content = await _repository.GetAsync(CancellationToken.None);
        await Assert.That(content).IsEqualTo(Content);
    }

    [Test]
    public async Task GetAsyncShouldReturnNullWhenContentIsNull()
    {
        _client.GetAsync(CancellationToken.None).Returns((string?)null);

        string? content = await _repository.GetAsync(CancellationToken.None);
        await Assert.That(content).IsNull();
    }
}
