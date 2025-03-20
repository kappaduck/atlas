// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;

namespace Unit.Tests.Domain.Geography;

internal sealed class AreaTests
{
    [Test]
    public async Task CompareToShouldReturnSmallerWhenLeftIsSmallerThanRightArea()
    {
        Area left = new(0.0);
        Area right = new(1.0);

        Area.Size size = left.CompareTo(right);

        await Assert.That(size).IsEqualTo(Area.Size.Smaller);
    }

    [Test]
    public async Task CompareToShouldReturnEqualWhenLeftIsEqualToRightArea()
    {
        Area left = new(1.0);
        Area right = new(1.0);

        Area.Size size = left.CompareTo(right);

        await Assert.That(size).IsEqualTo(Area.Size.Same);
    }

    [Test]
    public async Task CompareToShouldReturnLargerWhenLeftAreaIsLargerThanRightArea()
    {
        Area left = new(1.0);
        Area right = new(0.0);

        Area.Size size = left.CompareTo(right);

        await Assert.That(size).IsEqualTo(Area.Size.Larger);
    }

    [Test]
    public async Task CompareToShouldReturnLargerWhenLeftAreaIsEpsilonAndRightIsZero()
    {
        Area left = new(double.Epsilon);
        Area right = new(0.0);

        Area.Size size = left.CompareTo(right);

        await Assert.That(size).IsEqualTo(Area.Size.Larger);
    }
}
