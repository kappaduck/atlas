// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;

namespace Unit.Tests.Domain.Geography;

internal sealed class DirectionTests
{
    private readonly Coordinate _canada = new(60, -95);
    private readonly Coordinate _italy = new(42.83333333, 12.83333333);
    private readonly Coordinate _fiji = new(-18, 175);

    [Test]
    public async Task CalculateShouldReturnTheDirectionInAngleBetweenTwoCoordinates()
    {
        double direction = Direction.Calculate(_canada, _italy);

        await Assert.That(direction).IsEqualTo(104.0);
    }

    [Test]
    public async Task CalculateShouldReturnZeroGivenSameCoordinates()
    {
        double direction = Direction.Calculate(_canada, _canada);

        await Assert.That(direction).IsEqualTo(0);
    }

    [Test]
    public async Task DirectionShouldGiveTheAngleInTheThirdQuadrantGivenCanadaToFiji()
    {
        double direction = Direction.Calculate(_canada, _fiji);

        await Assert.That(direction).IsEqualTo(223);
    }

    [Test]
    public async Task DirectionShouldGiveTheAngleInTheFirstQuadrantGivenCanadaToFiji()
    {
        double direction = Direction.Calculate(_fiji, _canada);

        await Assert.That(direction).IsEqualTo(43);
    }

    [Test]
    public async Task DirectionShouldGiveTheAngleInTheFirstQuadrantGivenLongitudeFor180Degrees()
    {
        Coordinate from = new(0, 0);
        Coordinate to = new(0, 180);

        double direction = Direction.Calculate(from, to);

        await Assert.That(direction).IsEqualTo(90);
    }
}
