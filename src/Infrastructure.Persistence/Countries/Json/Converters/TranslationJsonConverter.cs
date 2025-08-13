// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Languages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Persistence.Countries.Json.Converters;

internal sealed class TranslationJsonConverter : JsonConverter<IEnumerable<Translation>>
{
    public override IEnumerable<Translation> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<Translation> translations = [];

        reader.Read();
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            string language = SkipPropertyNameAndGetValue(ref reader);
            string country = SkipPropertyNameAndGetValue(ref reader);

            translations.Add(new Translation(ToLanguage(language), country));

            reader.Read();
            reader.Read();
        }

        return translations;

        static Language ToLanguage(string language) => language switch
        {
            "en" => Language.English,
            "fr" => Language.French,
            _ => throw new NotSupportedException($"Language '{language}' is not supported.")
        };

        static string SkipPropertyNameAndGetValue(ref Utf8JsonReader reader)
        {
            reader.Read();
            reader.Read();

            return reader.GetString()!;
        }
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<Translation> value, JsonSerializerOptions options)
        => throw new NotSupportedException($"{nameof(TranslationJsonConverter)} is only used for deserialization");
}
