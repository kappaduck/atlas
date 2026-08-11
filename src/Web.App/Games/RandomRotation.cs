// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Games;

/// <summary>
/// Hashes a 64-bit value into a well-distributed pseudo-random one using the
/// SplitMix64 finalizer. Sequential inputs (0, 1, 2, …) produce uncorrelated
/// outputs with avalanche behavior — flipping one input bit flips ~half the
/// output bits — which makes it suitable for deriving a stable daily value
/// from a day counter. The mapping is a bijection, so distinct inputs never
/// collide. Relies on unchecked (wrapping) overflow; do not call in a checked context.
/// https://rosettacode.org/wiki/Pseudo-random_numbers/Splitmix64
/// </summary>
internal static class RandomRotation
{
    private static readonly DateOnly Epoch = new(2026, 1, 1);

    public static double Get()
    {
        int day = DateOnly.FromDateTime(DateTime.Today).DayNumber - Epoch.DayNumber;

        return (int)(Mix((ulong)day) % 360);
    }

    private static ulong Mix(ulong x)
    {
        x += 0x9E3779B97F4A7C15UL;
        x = (x ^ (x >> 30)) * 0xBF58476D1CE4E5B9UL;
        x = (x ^ (x >> 27)) * 0x94D049BB133111EBUL;
        return x ^ (x >> 31);
    }
}
