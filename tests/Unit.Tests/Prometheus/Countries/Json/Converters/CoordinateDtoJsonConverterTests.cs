// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Countries.Json;
using Prometheus.Countries.Json.Converters;
using System.Text;
using System.Text.Json;
using TUnit.Assertions.AssertConditions.Throws;

namespace Unit.Tests.Prometheus.Countries.Json.Converters;

internal sealed class CoordinateDtoJsonConverterTests
{
    private const string Json = /*lang=json,strict*/"""{ "latlng": [ 42.83333333, 12.83333333 ] }""";

    private readonly JsonSerializerOptions _options = CountryDtoJsonContext.Default.Options;

    private readonly CoordinateDtoJsonConverter _converter = new();

    [Test]
    public async Task ReadShouldReturnTheLatitude()
    {
        Utf8JsonReader reader = CreateJsonReader();

        (double latitude, _) = _converter.Read(ref reader, typeof(CoordinateDto), _options);

        await Assert.That(latitude).IsEqualTo(42.83333333);
    }

    [Test]
    public async Task ReadShouldReturnTheLongitude()
    {
        Utf8JsonReader reader = CreateJsonReader();

        (_, double longitude) = _converter.Read(ref reader, typeof(CoordinateDto), _options);

        await Assert.That(longitude).IsEqualTo(12.83333333);
    }

    [Test]
    public async Task ReadShouldStopReadingAtEndArray()
    {
        Utf8JsonReader reader = CreateJsonReader();

        _converter.Read(ref reader, typeof(CoordinateDto), _options);

        await Assert.That(reader.TokenType).IsEqualTo(JsonTokenType.EndArray);
    }

    [Test]
    public async Task WriteShouldThrowNotSupportedException()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        await Assert.That(() => _converter.Write(writer, new CoordinateDto(0, 0), _options))
                    .ThrowsExactly<NotSupportedException>()
                    .WithMessage($"{nameof(CoordinateDtoJsonConverter)} is only used for deserialization");
    }

    private static Utf8JsonReader CreateJsonReader()
    {
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(Json));

        while (reader.TokenType != JsonTokenType.StartArray)
            reader.Read();

        return reader;
    }
}
