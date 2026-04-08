// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Caching;

internal interface ICache
{
    void Save<T>(string key, T value);

    bool TryGet<T>(string key, [NotNullWhen(true)] out T? value);
}
