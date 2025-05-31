// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application.Countries.Services;

[ExcludeFromCodeCoverage]
internal sealed class DateHash : IDateHash
{
    private const uint Prime = 16777619;
    private const uint Offset = 2166136261;

    /// <summary>
    /// Hashes the given date. The hash is deterministic and will always return the same value for the same date. It is based on the FNV-1a algorithm.
    /// https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function.
    /// </summary>
    /// <param name="date">The date to hash.</param>
    /// <returns>The hashed value.</returns>
    public uint Hash(DateOnly date)
    {
        ReadOnlySpan<char> input = date.ToString("yyyyMMdd");

        uint hash = Offset;

        foreach (char c in input)
        {
            hash ^= c;
            hash *= Prime;
        }

        return hash;
    }
}
