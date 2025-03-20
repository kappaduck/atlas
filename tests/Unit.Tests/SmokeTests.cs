// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Unit.Tests;

public sealed class SmokeTests
{
    [Test]
    public async Task AddShouldGiveTheGoodResult()
    {
        int result = Add(1, 2);

        await Assert.That(result).IsEqualTo(3);
    }

    private static int Add(int a, int b) => a + b;
}
