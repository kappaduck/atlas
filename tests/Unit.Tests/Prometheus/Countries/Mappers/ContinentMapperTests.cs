// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Mappers;
using TUnit.Assertions.AssertConditions.Throws;

namespace Unit.Tests.Prometheus.Countries.Mappers;

public sealed class ContinentMapperTests
{
    [Test]
    public async Task AsDomainShouldThrowExceptionWhenRegionIsUnknown()
    {
        await Assert.That(() => ((RegionDto)999).AsDomain(subRegion: null))
                    .ThrowsExactly<ArgumentException>()
                    .WithMessage("Unknown region or sub region: 999 -  (Parameter 'region')");
    }

    [Test]
    [Arguments(RegionDto.Africa, Continent.Africa)]
    [Arguments(RegionDto.Asia, Continent.Asia)]
    [Arguments(RegionDto.Europe, Continent.Europe)]
    [Arguments(RegionDto.Oceania, Continent.Oceania)]
    [Arguments(RegionDto.Antarctic, Continent.Antarctica)]
    internal async Task AsDomainShouldMapDtoToDomain(RegionDto dto, Continent expectedContinent)
    {
        Continent continent = dto.AsDomain(subRegion: null);

        await Assert.That(continent).IsEqualTo(expectedContinent);
    }

    [Test]
    [Arguments(RegionDto.Americas, SubRegionDto.NorthAmerica, Continent.NorthAmerica)]
    [Arguments(RegionDto.Americas, SubRegionDto.SouthAmerica, Continent.SouthAmerica)]
    [Arguments(RegionDto.Americas, SubRegionDto.CentralAmerica, Continent.CentralAmerica)]
    [Arguments(RegionDto.Americas, SubRegionDto.Caribbean, Continent.CentralAmerica)]
    internal async Task AsDomainShouldMapDtoToDomainWithSubRegion(RegionDto region, SubRegionDto subRegion, Continent expectedContinent)
    {
        Continent continent = region.AsDomain(subRegion);

        await Assert.That(continent).IsEqualTo(expectedContinent);
    }
}
