// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Options;
using Prometheus.Countries.Providers;
using Prometheus.Patch;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Countries;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddCountries(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<CountryEndpointOptions>(builder.Configuration.GetSection(CountryEndpointOptions.Section))
                        .AddSingleton<IValidateOptions<CountryEndpointOptions>, CountryEndpointOptions.Validator>()
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryEndpointOptions>>().Value)
                        .AddOptionsWithValidateOnStart<CountryEndpointOptions>();

        builder.Services.Configure<CountryFilterOptions>(builder.Configuration.GetSection(CountryFilterOptions.Section))
                        .AddSingleton<IValidateOptions<CountryFilterOptions>, CountryFilterOptions.Validator>()
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryFilterOptions>>().Value)
                        .AddOptionsWithValidateOnStart<CountryFilterOptions>();

        builder.Services.AddHttpClient<ICountryProvider, CountryEndpoint>();
        builder.Services.AddTransient<IPatch<Span<CountryDto>>, CountryPatch>();
        builder.Services.AddTransient<IMigration, CountryMigration>();
    }
}
