// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Infrastructure.Persistence.Countries.Sources;

internal interface IDataSource<T>
{
    Task<T[]> QueryAllAsync(CancellationToken cancellationToken);
}
