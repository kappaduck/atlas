// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prometheus.Countries.Json.Converters;

internal sealed class TranslationDtoJsonConverter : JsonConverter<IEnumerable<TranslationDto>>
{
    public override IEnumerable<TranslationDto> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<TranslationDto> translations = [];

        while (reader.TokenType != JsonTokenType.PropertyName)
        {
            reader.Read();

            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            string code = reader.GetString()!;
            reader.Read();

            SkipPropertyNameAndGetValue(ref reader);
            string common = SkipPropertyNameAndGetValue(ref reader);

            translations.Add(new TranslationDto(code, common));

            reader.Read();
        }

        return translations;

        static string SkipPropertyNameAndGetValue(ref Utf8JsonReader reader)
        {
            reader.Read();
            reader.Read();

            return reader.GetString()!;
        }
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<TranslationDto> value, JsonSerializerOptions options)
        => throw new NotSupportedException($"{nameof(TranslationDtoJsonConverter)} is only used for deserialization");
}
