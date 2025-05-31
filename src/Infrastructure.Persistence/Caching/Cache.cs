// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Caching;

[ExcludeFromCodeCoverage]
internal sealed class Cache(IMemoryCache memory, CacheOptions options) : ICache
{
    public void Save<T>(string key, T value)
        => memory.Set(key, value, TimeSpan.FromMinutes(options.ExpirationTimeInMinutes));

    public bool TryGet<T>(string key, [NotNullWhen(true)] out T? value)
        => memory.TryGetValue(key, out value);
}
