// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;

namespace Atlas.Application.Changelog.Queries;

public interface IGetChangelog
{
    ValueTask<string?> HandleAsync(CancellationToken cancellationToken);
}

internal sealed class GetChangelog(IChangelogRepository repository) : IGetChangelog
{
    public ValueTask<string?> HandleAsync(CancellationToken cancellationToken)
        => repository.GetAsync(cancellationToken);
}
