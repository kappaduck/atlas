// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Integration.Tests;

public sealed class SmokeTests
{
    [Test]
    public async Task SubtractShouldGiveTheGoodResult()
    {
        int result = Subtract(5, 2);

        await Assert.That(result).IsEqualTo(3);
    }

    private static int Subtract(int a, int b) => a - b;
}
