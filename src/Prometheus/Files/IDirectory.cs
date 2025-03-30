// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Prometheus.Files;

internal interface IDirectory
{
    string Create(string path);

    string? GetRootPath(string rootToFind);
}
