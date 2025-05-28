// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Countries.Json;
using Prometheus.Countries.Json.Converters;
using System.Text;
using System.Text.Json;
using TUnit.Assertions.AssertConditions.Throws;

namespace Unit.Tests.Prometheus.Countries.Json.Converters;

internal sealed class TranslationDtoJsonConverterTests
{
    private const string Json = /*lang=json,strict*/"""{ "translations": { "fra": { "official": "Official", "common": "Canada" }, "swe": { "official": "Official", "common": "Kanada" } } }""";

    private readonly JsonSerializerOptions _options = CountryDtoJsonContext.Default.Options;
    private readonly TranslationDtoJsonConverter _converter = new();

    [Test]
    public async Task ReadShouldReturnAListOfTranslations()
    {
        Utf8JsonReader reader = CreateJsonReader(Json);

        IEnumerable<TranslationDto> translations = _converter.Read(ref reader, typeof(IEnumerable<TranslationDto>), _options);

        await Assert.That(translations).HasCount().EqualTo(2);

        await Assert.That(translations).Contains(new TranslationDto("fra", "Canada"));
        await Assert.That(translations).Contains(new TranslationDto("swe", "Kanada"));
    }

    [Test]
    public async Task ReadShouldReturnEmptyListWhenThereIsNoTranslations()
    {
        const string json = /*lang=json,strict*/"""{ "translations": {} }""";

        Utf8JsonReader reader = CreateJsonReader(json);

        IEnumerable<TranslationDto> translations = _converter.Read(ref reader, typeof(IEnumerable<TranslationDto>), _options);

        await Assert.That(translations).IsEmpty();
    }

    [Test]
    public async Task ReadShouldStopReadingAtEndObject()
    {
        Utf8JsonReader reader = CreateJsonReader(Json);

        _converter.Read(ref reader, typeof(IEnumerable<TranslationDto>), _options);

        await Assert.That(reader.TokenType).IsEqualTo(JsonTokenType.EndObject);
    }

    [Test]
    public async Task WriteShouldThrowNotSupportedException()
    {
        await using MemoryStream stream = new();
        await using Utf8JsonWriter writer = new(stream);

        await Assert.That(() => _converter.Write(writer, [], _options))
                    .ThrowsExactly<NotSupportedException>()
                    .WithMessage($"{nameof(TranslationDtoJsonConverter)} is only used for deserialization");
    }

    private static Utf8JsonReader CreateJsonReader(string json)
    {
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        reader.Read();
        reader.Read();

        while (reader.TokenType != JsonTokenType.StartObject)
            reader.Read();

        return reader;
    }
}
