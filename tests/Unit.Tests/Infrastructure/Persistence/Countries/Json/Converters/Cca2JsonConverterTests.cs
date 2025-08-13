// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Json;
using Infrastructure.Persistence.Countries.Json.Converters;
using System.Text;
using System.Text.Json;
using TUnit.Assertions.AssertConditions.Throws;

namespace Unit.Tests.Infrastructure.Persistence.Countries.Json.Converters;

public sealed class Cca2JsonConverterTests
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
    public async Task WriteShouldThrowNotSupportedException()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        await Assert.That(() => _converter.Write(writer, new Cca2("CA"), _options))
                    .ThrowsExactly<NotSupportedException>()
                    .WithMessage($"{nameof(Cca2JsonConverter)} is only used for deserialization");
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
