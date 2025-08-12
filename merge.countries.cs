#!/usr/bin/env dotnet

// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

const string directory = "countries";
const string mergedFile = $"{directory}/all.json";
const string lookupFile = $"{directory}/lookup.json";

File.Delete(mergedFile);
File.Delete(lookupFile);

List<Country> countries = [];

foreach (string filePath in Directory.EnumerateFiles(directory, "*.json", SearchOption.AllDirectories))
{
    using Stream stream = File.OpenRead(filePath);

    Country country = (await JsonSerializer.DeserializeAsync(stream, JsonContext.Default.Country))!;

    Console.WriteLine($"Merging {country.Cca2}");
    countries.Add(country);
}

using Stream mergedStream = File.OpenWrite(mergedFile);
await JsonSerializer.SerializeAsync(mergedStream, countries, JsonContext.Default.ListCountry);

using Stream lookupStream = File.OpenWrite(lookupFile);
await JsonSerializer.SerializeAsync(lookupStream, countries.Select(ToLookup), JsonContext.Default.IEnumerableCountryLookup);

static CountryLookup ToLookup(Country country) => new()
{
    Cca2 = country.Cca2,
    Translations = country.Translations
};

[JsonSerializable(typeof(Country))]
[JsonSerializable(typeof(CountryLookup), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(List<Country>), GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(IEnumerable<CountryLookup>), GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class JsonContext : JsonSerializerContext;

internal sealed record Country
{
    public required string Cca2 { get; init; }

    public required IEnumerable<Translation> Translations { get; init; }

    public required IEnumerable<Capital> Capitals { get; init; }

    public required IEnumerable<string> Borders { get; init; }

    public required string Continent { get; init; }

    public required Coordinate Coordinate { get; init; }

    public required double Area { get; init; }

    public required int Population { get; init; }

    public required Resources Resources { get; init; }
}

internal sealed record CountryLookup
{
    public required string Cca2 { get; init; }

    public required IEnumerable<Translation> Translations { get; init; }
}

internal sealed record Translation(string Language, string Name);

internal sealed record Capital(string Name, Coordinate Coordinate);

internal sealed record Coordinate(double Latitude, double Longitude);

internal sealed record Resources
{
    public required Uri Map { get; init; }

    public required Uri Flag { get; init; }

    public Uri? CoatOfArms { get; init; }
}
