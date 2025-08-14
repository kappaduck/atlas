// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;
using Infrastructure.Persistence.Countries.Json;
using Infrastructure.Persistence.Countries.Json.Converters;
using System.Text;
using System.Text.Json;

namespace Unit.Tests.Infrastructure.Persistence.Countries.Json.Converters;

public sealed class AreaJsonConverterTests
{
    private readonly JsonSerializerOptions _options = CountryJsonContext.Default.Options;
    private readonly AreaJsonConverter _converter = new();

    [Test]
    public async Task ReadShouldGetArea()
    {
        Utf8JsonReader reader = CreateJsonReader();

        double area = _converter.Read(ref reader, typeof(Area), _options);

        await Assert.That(area).IsEqualTo(42);
    }

    [Test]
    public async Task WriteShouldThrowNotSupportedException()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        await Assert.That(() => _converter.Write(writer, new Area(42.0), _options))
                    .ThrowsExactly<NotSupportedException>()
                    .WithMessage($"{nameof(AreaJsonConverter)} is only used for deserialization");
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
