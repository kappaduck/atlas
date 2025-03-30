// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Mappers;

namespace Unit.Tests.Prometheus.Countries.Mappers;

public sealed class CapitalMapperTests
{
    private readonly CapitalInfoDto _capitalDto = new() { Coordinate = new CoordinateDto(42, 42) };
    private readonly CoordinateDto _coordinateDto = new(0, 90);

    [Test]
    public async Task AsDomainShouldMapDtoToDomain()
    {
        Capital[] capitals = _capitalDto.AsDomain(["Ottawa"], _coordinateDto);

        Capital capital = capitals[0];

        await Assert.That(capital.Name).IsEqualTo("Ottawa");
        await Assert.That(capital.Coordinate.Latitude).IsEqualTo(_capitalDto.Coordinate!.Latitude);
        await Assert.That(capital.Coordinate.Longitude).IsEqualTo(_capitalDto.Coordinate.Longitude);
    }

    [Test]
    public async Task AsDomainShouldReturnEmptyWhenThereIsNoCapitals()
    {
        CapitalInfoDto dto = new();

        Capital[] capitals = dto.AsDomain(capitals: null, _coordinateDto);

        await Assert.That(capitals).IsEmpty();
    }

    [Test]
    public async Task AsDomainShouldMapWithFallbackCoordinateWhenCapitalInfoCoordinateIsNull()
    {
        CapitalInfoDto capitalInfoWithoutCoordinate = _capitalDto with { Coordinate = null };

        Capital[] capitals = capitalInfoWithoutCoordinate.AsDomain(["Ottawa"], _coordinateDto);

        Capital capital = capitals[0];

        await Assert.That(capital.Name).IsEqualTo("Ottawa");
        await Assert.That(capital.Coordinate.Latitude).IsEqualTo(_coordinateDto.Latitude);
        await Assert.That(capital.Coordinate.Longitude).IsEqualTo(_coordinateDto.Longitude);
    }
}
