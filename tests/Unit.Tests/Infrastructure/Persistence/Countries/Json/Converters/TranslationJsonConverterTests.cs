// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Languages;
using Infrastructure.Persistence.Countries.Json;
using Infrastructure.Persistence.Countries.Json.Converters;
using System.Text;
using System.Text.Json;
using TUnit.Assertions.AssertConditions.Throws;

namespace Unit.Tests.Infrastructure.Persistence.Countries.Json.Converters;

public sealed class TranslationJsonConverterTests
{
    private const string Json = /*lang=json,strict*/"""
    {
        "translations": [
            {
                "language": "en",
                "name": "Hello"
            },
            {
                "language": "fr",
                "name": "Bonjour"
            }
        ]
    }
    """;

    private readonly JsonSerializerOptions _options = CountryJsonContext.Default.Options;
    private readonly TranslationJsonConverter _converter = new();

    [Test]
    public async Task ReadShouldReturnAListOfTranslations()
    {
        Utf8JsonReader reader = CreateJsonReader(Json);

        IEnumerable<Translation> translations = _converter.Read(ref reader, typeof(IEnumerable<Translation>), _options);

        await Assert.That(translations).HasCount(2);
        await Assert.That(translations).Contains(new Translation(Language.English, "Hello"));
        await Assert.That(translations).Contains(new Translation(Language.French, "Bonjour"));
    }

    [Test]
    public async Task WriteShouldThrowNotSupportedException()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        await Assert.That(() => _converter.Write(writer, [], _options))
                    .ThrowsExactly<NotSupportedException>()
                    .WithMessage($"{nameof(TranslationJsonConverter)} is only used for deserialization");
    }

    private static Utf8JsonReader CreateJsonReader(string json)
    {
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.TokenType != JsonTokenType.StartArray)
            reader.Read();

        return reader;
    }
}
