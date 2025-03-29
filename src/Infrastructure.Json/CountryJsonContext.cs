// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Json.Converters;
using System.Text.Json.Serialization;

namespace Infrastructure.Json;

[JsonSerializable(typeof(Country[]))]
[JsonSerializable(typeof(CountryLookup[]))]
[JsonSourceGenerationOptions(
    Converters = [typeof(AreaJsonConverter), typeof(Cca2JsonConverter)],
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
public sealed partial class CountryJsonContext : JsonSerializerContext;
