// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application;
using Atlas.Application.Countries;

namespace Atlas.Application.Countries;

internal static class DateTimeExtensions
{
    extension(DateTime date)
    {
        /// <summary>
        /// Hashes the given date. The hash is deterministic and will always return the same value for the same date. It is based on the FNV-1a algorithm.
        /// https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function.
        /// </summary>
        /// <param name="key">The key to hash with the date.</param>
        /// <returns>The hashed value.</returns>
        internal uint Hash(string key)
        {
            const uint prime = 16777619;
            uint hash = 2166136261;

            foreach (char c in $"{key}:{date:yyyyMMdd}".AsSpan())
            {
                hash ^= c;
                hash *= prime;
            }

            return hash;
        }
    }
}
