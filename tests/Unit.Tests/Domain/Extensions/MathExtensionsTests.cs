// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Extensions;

namespace Unit.Tests.Domain.Extensions;

internal sealed class MathExtensionsTests
{
    [Test]
    public async Task ToRadiansShouldConvertDegreesToRadians()
    {
        const double degree = 180.0;

        double actual = degree.ToRadians();

        await Assert.That(actual).IsEqualTo(Math.PI);
    }

    [Test]
    public async Task ToDegreesShouldConvertRadiansToDegrees()
    {
        const double radian = Math.PI;

        double actual = radian.ToDegrees();

        await Assert.That(actual).IsEqualTo(180.0);
    }

    [Test]
    [Arguments(-270, 90.0)]
    [Arguments(-180, 180.0)]
    [Arguments(-90, 270.0)]
    [Arguments(0.0, 0.0)]
    [Arguments(90.0, 90.0)]
    [Arguments(180.0, 180.0)]
    [Arguments(360.0, 0.0)]
    public async Task NormalizeShouldAdaptTheAngleIn360Degrees(double angle, double normalizedAngle)
    {
        double actual = angle.Normalize();

        await Assert.That(actual).IsEqualTo(normalizedAngle);
    }
}
