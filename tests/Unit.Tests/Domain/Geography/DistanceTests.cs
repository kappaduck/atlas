// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;

namespace Unit.Tests.Domain.Geography;

public sealed class DistanceTests
{
    private readonly Coordinate _canada = new(60.0, -95.0);
    private readonly Coordinate _italy = new(42.83333333, 12.83333333);

    [Test]
    public async Task CalculateShouldReturnTheDistanceBetweenTwoCoordinates()
    {
        Distance distance = Distance.Calculate(_canada, _italy);

        await Assert.That(distance.Kilometers).IsEqualTo(6843.3).Within(0.1);
        await Assert.That(distance.Miles).IsEqualTo(4252.2).Within(0.1);
    }

    [Test]
    public async Task CalculateShouldReturnZeroGivenSameCoordinates()
    {
        Distance distance = Distance.Calculate(_canada, _canada);

        await Assert.That(distance.Kilometers).IsEqualTo(0);
        await Assert.That(distance.Miles).IsEqualTo(0);
    }
}
