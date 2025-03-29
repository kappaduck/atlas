// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Infrastructure.Json.Converters;
using System.Text;
using System.Text.Json;

namespace Unit.Tests.Infrastructure.Json.Converters;

internal sealed class AreaJsonConverterTests
{
    private readonly JsonSerializerOptions _options = new();
    private readonly AreaJsonConverter _converter = new();

    [Test]
    public async Task ReadShouldGetArea()
    {
        Utf8JsonReader reader = CreateJsonReader();

        double area = _converter.Read(ref reader, typeof(Area), _options);

        await Assert.That(area).IsEqualTo(42);
    }

    [Test]
    public async Task WriteShouldWriteAreaToJson()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        writer.WriteStartObject();
        writer.WritePropertyName("area");

        _converter.Write(writer, new Area(42.0), _options);

        writer.WriteEndObject();
        await writer.FlushAsync();

        string json = Encoding.UTF8.GetString(stream.ToArray());
        await Assert.That(json).IsEqualTo(/*lang=json,strict*/ """{"area":42}""");
    }

    private static Utf8JsonReader CreateJsonReader()
    {
        const string json = /*lang=json,strict*/"""{ "area": 42 }""";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.TokenType != JsonTokenType.Number)
            reader.Read();

        return reader;
    }
}
