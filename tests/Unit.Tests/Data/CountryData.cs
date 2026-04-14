// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Json;
using System.Text.Json;

namespace Unit.Tests.Data;

public sealed class CountryData
{
    public CountryData()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "../../../../../countries");

        All = File.ReadAllText(Path.Combine(path, "all.json"));
        Lookup = File.ReadAllText(Path.Combine(path, "lookup.json"));

        string canadaJson = File.ReadAllText(Path.Combine(path, "ca/country.json"));
        Canada = JsonSerializer.Deserialize(canadaJson, CountryJsonContext.Default.Country)!;

        string italyJson = File.ReadAllText(Path.Combine(path, "it/country.json"));
        Italy = JsonSerializer.Deserialize(italyJson, CountryJsonContext.Default.Country)!;
    }

    public string All { get; }

    public string Lookup { get; }

    public Country Canada { get; }

    public Country Italy { get; }
}
