// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Infrastructure.Json;
using Prometheus.Files.Options;

namespace Prometheus.Files;

internal sealed class DataDirectory(IDirectory directory, DataPathOptions options) : IDataDirectory
{
    public string? Create()
    {
        string? rootPath = directory.GetRootPath(options.Root);

        if (string.IsNullOrEmpty(rootPath))
            return null;

        string dataPath = Path.Combine(rootPath, options.Output, DataJsonPaths.BaseDirectory);

        return directory.Create(dataPath);
    }
}
