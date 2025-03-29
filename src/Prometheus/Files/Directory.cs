// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Prometheus.Files;

internal sealed class Directory : IDirectory
{
    public string Create(string path) => System.IO.Directory.CreateDirectory(path).FullName;

    public string? GetRootPath(string rootToFind)
    {
        DirectoryInfo current = new(System.IO.Directory.GetCurrentDirectory());

        while (current.Parent is not null)
        {
            if (rootToFind.Equals(current.Name.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return current.FullName;

            current = current.Parent;
        }

        return null;
    }
}
