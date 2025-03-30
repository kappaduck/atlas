// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Prometheus.Countries.Mappers;

internal static class CountryLookupMapper
{
    internal static CountryLookup[] AsLookups(this Country[] countries)
        => [.. countries.Select(AsLookup)];

    private static CountryLookup AsLookup(this Country country) => new()
    {
        Cca2 = country.Cca2,
        Translations = country.Translations,
        IsExcluded = country.IsExcluded
    };
}
