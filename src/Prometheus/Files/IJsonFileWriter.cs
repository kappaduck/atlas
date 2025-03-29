// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Text.Json.Serialization.Metadata;

namespace Prometheus.Files;

internal interface IJsonFileWriter
{
    Task WriteToAsync<T>(string path, T value, JsonTypeInfo<T> metadata, CancellationToken cancellationToken);
}
