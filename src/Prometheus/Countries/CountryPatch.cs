// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;
using Prometheus.Patch;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Countries;

[ExcludeFromCodeCoverage]
internal sealed class CountryPatch : IPatch<Span<CountryDto>>
{
    public Span<CountryDto> ApplyTo(Span<CountryDto> target)
    {
        Dictionary<string, IPatch<CountryDto>> patches = GetCountryPatches();

        foreach (ref CountryDto country in target)
        {
            if (patches.TryGetValue(country.Cca2, out IPatch<CountryDto>? patch))
                country = patch.ApplyTo(country);
        }

        return target;
    }

    private static Dictionary<string, IPatch<CountryDto>> GetCountryPatches() => new(StringComparer.OrdinalIgnoreCase)
    {
        ["cc"] = new CocoIslandsPatch(),
        ["hm"] = new HeardIslandMcDonaldIslandsPatch(),
        ["pf"] = new FrenchPolynesiaPatch(),
        ["mf"] = new SaintMartinPatch(),
        ["fj"] = new FijiPatch()
    };

    private sealed class CocoIslandsPatch : IPatch<CountryDto>
    {
        public CountryDto ApplyTo(CountryDto target)
        {
            return target with
            {
                Coordinate = target.Coordinate with { Latitude = -12.1642 }
            };
        }
    }

    private sealed class HeardIslandMcDonaldIslandsPatch : IPatch<CountryDto>
    {
        public CountryDto ApplyTo(CountryDto target)
        {
            return target with
            {
                Coordinate = target.Coordinate with { Latitude = -53.0818 }
            };
        }
    }

    private sealed class FrenchPolynesiaPatch : IPatch<CountryDto>
    {
        public CountryDto ApplyTo(CountryDto target)
        {
            return target with
            {
                Coordinate = target.Coordinate with
                {
                    Latitude = -17.6797,
                    Longitude = -149.4068
                }
            };
        }
    }

    private sealed class SaintMartinPatch : IPatch<CountryDto>
    {
        public CountryDto ApplyTo(CountryDto target)
        {
            return target with
            {
                Coordinate = target.Coordinate with { Longitude = -63.0501 }
            };
        }
    }

    private sealed class FijiPatch : IPatch<CountryDto>
    {
        public CountryDto ApplyTo(CountryDto target)
        {
            return target with
            {
                Coordinate = target.Coordinate with { Latitude = -17.7134 }
            };
        }
    }
}
