// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Unit.Tests.Domain.Countries;

public sealed class Cca2Tests
{
    private readonly Cca2 _cca2 = new("CA");

    [Test]
    [Arguments("CA")]
    [Arguments("ca")]
    [Arguments("cA")]
    [Arguments("Ca")]
    public async Task Cca2ShouldBeEquivalentToAnyCase(string cca2)
    {
        bool result = _cca2 == new Cca2(cca2);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Cca2ShouldBeFalseGivenDifferentCca2()
    {
        bool result = _cca2 == new Cca2("JP");
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ToStringShouldGiveTheCode()
    {
        string code = _cca2.ToString();
        await Assert.That(code).IsEqualTo(_cca2.Code);
    }

    [Test]
    [Arguments("CA")]
    [Arguments("ca")]
    [Arguments("cA")]
    [Arguments("Ca")]
    public async Task GetHashCodeShouldReturnCca2HashCode(string cca2)
    {
        int hashCode = new Cca2(cca2).GetHashCode();
        await Assert.That(hashCode).IsEqualTo(_cca2.GetHashCode());
    }
}
