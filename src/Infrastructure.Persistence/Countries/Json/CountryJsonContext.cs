// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Json.Converters;
using System.Text.Json.Serialization;

namespace Infrastructure.Persistence.Countries.Json;

[JsonSerializable(typeof(Country[]))]
[JsonSerializable(typeof(CountryLookup[]))]
[JsonSourceGenerationOptions(
    Converters = [typeof(AreaJsonConverter), typeof(Cca2JsonConverter)],
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
internal sealed partial class CountryJsonContext : JsonSerializerContext;
