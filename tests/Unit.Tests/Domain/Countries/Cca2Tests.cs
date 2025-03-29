// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Unit.Tests.Domain.Countries;

internal sealed class Cca2Tests
{
    private readonly Cca2 _cca2 = new("CA");

    [Test]
    [Arguments("CA", true)]
    [Arguments("ca", true)]
    [Arguments("cA", true)]
    [Arguments("Ca", true)]
    [Arguments("JP", false)]
    public async Task Cca2ShouldBeEqualEvenThereIsDifferentCase(string cca2, bool result)
    {
        bool isEqual = _cca2 == new Cca2(cca2);
        await Assert.That(isEqual).IsEqualTo(result);
    }
}
