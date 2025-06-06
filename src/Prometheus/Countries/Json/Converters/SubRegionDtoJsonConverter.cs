// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prometheus.Countries.Json.Converters;

internal sealed class SubRegionDtoJsonConverter : JsonConverter<SubRegionDto?>
{
    public override SubRegionDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? subRegion = reader.GetString();

        return subRegion switch
        {
            "North America" => SubRegionDto.NorthAmerica,
            "South America" => SubRegionDto.SouthAmerica,
            "Central America" => SubRegionDto.CentralAmerica,
            "Caribbean" => SubRegionDto.Caribbean,
            _ => null
        };
    }

    public override void Write(Utf8JsonWriter writer, SubRegionDto? value, JsonSerializerOptions options)
        => throw new NotSupportedException($"{nameof(SubRegionDtoJsonConverter)} is only used for deserialization");
}
