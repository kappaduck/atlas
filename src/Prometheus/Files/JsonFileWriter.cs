// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Prometheus.Files;

[ExcludeFromCodeCoverage]
internal sealed class JsonFileWriter : IJsonFileWriter
{
    public async Task WriteToAsync<T>(string path, T value, JsonTypeInfo<T> metadata, CancellationToken cancellationToken)
    {
        Stream stream = File.Open(path, FileMode.Create);

        await using (stream.ConfigureAwait(false))
        {
            await JsonSerializer.SerializeAsync(stream, value, metadata, cancellationToken).ConfigureAwait(false);
        }
    }
}
