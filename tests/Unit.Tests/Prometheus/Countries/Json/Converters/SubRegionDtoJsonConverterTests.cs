// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Countries.Json.Converters;
using System.Text;
using System.Text.Json;
using TUnit.Assertions.AssertConditions.Throws;

namespace Unit.Tests.Prometheus.Countries.Json.Converters;

internal sealed class SubRegionDtoJsonConverterTests
{
    private readonly JsonSerializerOptions _options = CountryDtoJsonContext.Default.Options;

    private readonly SubRegionDtoJsonConverter _converter = new();

    [Test]
    [Arguments("Asphodel Meadows", null)]
    [Arguments("North America", SubRegionDto.NorthAmerica)]
    [Arguments("South America", SubRegionDto.SouthAmerica)]
    [Arguments("Central America", SubRegionDto.CentralAmerica)]
    [Arguments("Caribbean", SubRegionDto.Caribbean)]
    public async Task ReadShouldReturnTheGoodSubRegion(string value, SubRegionDto? expected)
    {
        string json = /*lang=json,strict*/$@"{{ ""subregion"": ""{value}"" }}";

        Utf8JsonReader reader = CreateJsonReader(json);

        SubRegionDto? subRegion = _converter.Read(ref reader, typeof(SubRegionDto), _options);

        await Assert.That(subRegion).IsEqualTo(expected);
    }

    [Test]
    public async Task WriteShouldThrowNotSupportedException()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        await Assert.That(() => _converter.Write(writer, value: null, _options))
                    .ThrowsExactly<NotSupportedException>()
                    .WithMessage($"{nameof(SubRegionDtoJsonConverter)} is only used for deserialization");
    }

    private static Utf8JsonReader CreateJsonReader(string json)
    {
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.TokenType != JsonTokenType.String)
            reader.Read();

        return reader;
    }
}
