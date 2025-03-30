// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prometheus.Countries.Options;
using Prometheus.Countries.Providers;
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

        builder.Services.AddHttpClient<ICountryProvider, CountryEndpoint>();
    }
}
