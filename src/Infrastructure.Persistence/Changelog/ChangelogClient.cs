// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Infrastructure.Persistence.Changelog;

internal sealed class ChangelogClient(HttpClient http, ChangelogEndpointOptions options) : IChangelogClient
{
    public async Task<string?> GetAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await http.GetAsync(options.Url, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        string content = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrEmpty(content) ? null : content;
    }
}
