// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Options;
using Infrastructure.Persistence.Countries.Sources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Countries;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        internal void AddCountries(Action<HttpClient> configure, IConfiguration configuration)
        {
            services.Configure<CountryEndpointOptions>(configuration.GetSection(CountryEndpointOptions.Section))
                    .AddSingleton<IValidateOptions<CountryEndpointOptions>, CountryEndpointOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryEndpointOptions>>().Value)
                    .AddOptionsWithValidateOnStart<CountryEndpointOptions>();

            services.Configure<ExcludedCountriesOptions>(configuration.GetSection(ExcludedCountriesOptions.Section))
                    .AddSingleton<IValidateOptions<ExcludedCountriesOptions>, ExcludedCountriesOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<ExcludedCountriesOptions>>().Value)
                    .AddOptionsWithValidateOnStart<ExcludedCountriesOptions>();

            services.AddHttpClient<IDataSource<Country>, CountryDataSource>(configure);
            services.AddHttpClient<IDataSource<Cca2>, CountryLookupDataSource>(configure);

            services.AddSingleton<ICountryRepository, CountryRepository>();
            services.AddSingleton<ICountryLookupRepository, CountryLookupRepository>();
        }
    }
}
