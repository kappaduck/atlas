// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Changelog.Sources;

[ExcludeFromCodeCoverage]
internal sealed class ChangelogClient(HttpClient client, ChangelogOptions options) : IChangelogClient
{
    public async Task<string?> GetAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await client.GetAsync(options.Url, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }
}
