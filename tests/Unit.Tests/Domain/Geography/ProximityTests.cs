// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;

namespace Unit.Tests.Domain.Geography;

public sealed class ProximityTests
{
    private readonly Coordinate _canada = new(60.0, -95.0);
    private readonly Coordinate _italy = new(42.83333333, 12.83333333);
    private readonly Coordinate _fiji = new(-18.0, 175.0);
    private readonly Coordinate _spain = new(40.0, -4.0);

    [Test]
    public async Task CalculateShouldReturnFullProximityGivenSameCoordinates()
    {
        int proximity = Proximity.Calculate(_canada, _canada);
        await Assert.That(proximity).IsEqualTo(100);
    }

    [Test]
    public async Task CalculateShouldReturnZeroProximityGivenAntipodalCoordinates()
    {
        Coordinate from = new(0.0, 0.0);
        Coordinate to = new(0.0, 180.0);

        int proximity = Proximity.Calculate(from, to);
        await Assert.That(proximity).IsEqualTo(0);
    }

    [Test]
    public async Task CalculateShouldReturnTheProximityBetweenTwoCoordinates()
    {
        int proximity = Proximity.Calculate(_canada, _italy);
        await Assert.That(proximity).IsEqualTo(66);
    }

    [Test]
    public async Task CalculateShouldReturnLowerProximityGivenFartherCoordinates()
    {
        int proximity = Proximity.Calculate(_canada, _fiji);
        await Assert.That(proximity).IsEqualTo(41);
    }

    [Test]
    public async Task CalculateShouldBeSymmetric()
    {
        int forward = Proximity.Calculate(_spain, _canada);
        int backward = Proximity.Calculate(_canada, _spain);

        await Assert.That(forward).IsEqualTo(69);
        await Assert.That(backward).IsEqualTo(forward);
    }
}
