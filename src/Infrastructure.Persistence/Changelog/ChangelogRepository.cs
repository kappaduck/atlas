// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Changelog.Sources;

namespace Infrastructure.Persistence.Changelog;

internal sealed class ChangelogRepository(IChangelogClient client, ICache cache) : IChangelogRepository
{
    private const string Key = "changelog";

    public async ValueTask<string?> GetAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGet(Key, out string? cachedChangelog))
            return cachedChangelog;

        string? changelog = await client.GetAsync(cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrEmpty(changelog))
            return null;

        cache.Save(Key, changelog);
        return changelog;
    }
}
