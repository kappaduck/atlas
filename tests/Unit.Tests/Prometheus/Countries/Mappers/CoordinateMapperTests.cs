// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Mappers;

namespace Unit.Tests.Prometheus.Countries.Mappers;

public sealed class CoordinateMapperTests
{
    [Test]
    public async Task AsDomainShouldMapDtoToDomain()
    {
        CoordinateDto dto = new(1.0, 2.0);
        Coordinate coordinate = dto.AsDomain();

        await Assert.That(coordinate.Latitude).IsEqualTo(1.0);
        await Assert.That(coordinate.Longitude).IsEqualTo(2.0);
    }
}
