// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public static class Proximity
{
    public static int Calculate(Coordinate from, Coordinate to)
    {
        Distance distance = Distance.Calculate(from, to);

        double ratio = 1.0 - (distance.CentralAngle / Math.PI);
        return (int)Math.Round(ratio * 100, MidpointRounding.AwayFromZero);
    }
}
