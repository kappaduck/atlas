// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Geography;

public sealed class Distance
{
    private const double EarthRadiusKilometers = 6371.0;
    private const double EarthRadiusMiles = 3958.8;

    private Distance(double kilometers, double miles)
    {
        Kilometers = kilometers;
        Miles = miles;
    }

    public double Kilometers { get; }

    public double Miles { get; }

    public static Distance Calculate(Coordinate from, Coordinate to)
    {
        if (from == to)
            return new(0.0, 0.0);

        double deltaLatitude = double.DegreesToRadians(to.Latitude - from.Latitude);
        double deltaLongitude = double.DegreesToRadians(to.Longitude - from.Longitude);
        double fromLatitude = double.DegreesToRadians(from.Latitude);
        double toLatitude = double.DegreesToRadians(to.Latitude);

        double sinLatitude = Math.Sin(deltaLatitude / 2);
        double sinLongitude = Math.Sin(deltaLongitude / 2);

        double a = (sinLatitude * sinLatitude) + (sinLongitude * sinLongitude * Math.Cos(fromLatitude) * Math.Cos(toLatitude));
        double c = 2 * Math.Asin(Math.Sqrt(a));

        return new(c * EarthRadiusKilometers, c * EarthRadiusMiles);
    }
}
