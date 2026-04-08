// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Infrastructure.Persistence.Changelog;

internal interface IChangelogClient
{
    Task<string?> GetAsync(CancellationToken cancellationToken);
}
