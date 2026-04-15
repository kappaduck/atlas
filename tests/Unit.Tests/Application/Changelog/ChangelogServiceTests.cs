// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog;

namespace Unit.Tests.Application.Changelog;

public sealed class ChangelogServiceTests
{
    private const string Content = "changelog";

    private readonly Mock<IChangelogRepository> _repository = IChangelogRepository.Mock();
    private readonly ChangelogService _service;

    public ChangelogServiceTests()
    {
        _repository.GetAsync(CancellationToken.None).Returns(Content);
        _service = new ChangelogService(_repository.Object);
    }

    [Test]
    public async Task GetAsyncShouldCallRepository()
    {
        await _service.GetAsync(CancellationToken.None);
        _repository.GetAsync(CancellationToken.None).WasCalled(Times.Once);
    }

    [Test]
    public async Task GetAsyncShouldGetContent()
    {
        string? content = await _service.GetAsync(CancellationToken.None);
        await Assert.That(content).IsEqualTo(Content);
    }
}
