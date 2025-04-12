// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Extensions;

internal static class MathExtensions
{
    extension(Math)
    {
        internal static double ToRadians(double degree) => degree * Math.PI / 180.0;

        internal static double ToDegrees(double radian) => radian * (180 / Math.PI);

        internal static double Normalize(double angle) => (angle + 360) % 360;
    }
}
