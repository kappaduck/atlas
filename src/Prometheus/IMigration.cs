// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Prometheus;

internal interface IMigration
{
    string Name { get; }

    Task MigrateAsync(string path, CancellationToken cancellationToken);
}
