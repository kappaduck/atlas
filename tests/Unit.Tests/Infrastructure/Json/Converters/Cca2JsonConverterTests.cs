// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Json;
using Infrastructure.Json.Converters;
using System.Text;
using System.Text.Json;

namespace Unit.Tests.Infrastructure.Json.Converters;

internal sealed class Cca2JsonConverterTests
{
    private readonly JsonSerializerOptions _options = CountryJsonContext.Default.Options;
    private readonly Cca2JsonConverter _converter = new();

    [Test]
    public async Task ReadShouldGetCca2()
    {
        Utf8JsonReader reader = CreateJsonReader();

        Cca2 cca2 = _converter.Read(ref reader, typeof(Cca2), _options);

        await Assert.That(cca2).IsEqualTo(new Cca2("CA"));
    }

    [Test]
    public async Task WriteShouldWriteCca2ToJson()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        writer.WriteStartObject();
        writer.WritePropertyName("cca2");

        _converter.Write(writer, new Cca2("CA"), _options);

        writer.WriteEndObject();
        await writer.FlushAsync();

        string json = Encoding.UTF8.GetString(stream.ToArray());
        await Assert.That(json).IsEqualTo(/*lang=json,strict*/ """{"cca2":"CA"}""");
    }

    private static Utf8JsonReader CreateJsonReader()
    {
        const string json = /*lang=json,strict*/"""{ "cca2": "CA" }""";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.TokenType != JsonTokenType.String)
            reader.Read();

        return reader;
    }
}
