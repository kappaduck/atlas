// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Queries;
using Atlas.Application.Changelog.Repositories;

namespace Unit.Tests.Application.Changelog.Queries;

public sealed class GetChangelogTests
{
    private readonly IChangelogRepository _repository = Substitute.For<IChangelogRepository>();

    private readonly GetChangelog.Query _query = new();
    private readonly GetChangelog.Handler _handler;

    public GetChangelogTests()
    {
        _handler = new GetChangelog.Handler(_repository);
    }

    [Test]
    public async Task HandleShouldGetChangelog()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _repository.Received(1).GetAsync(CancellationToken.None);
    }
}
