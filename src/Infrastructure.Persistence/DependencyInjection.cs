// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Changelog;
using Infrastructure.Persistence.Countries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddPersistence(IConfiguration configuration)
        {
            services.AddCaching(configuration);
            services.AddChangelog(configuration);
            services.AddCountries(configuration);
        }

        private void AddCaching(IConfiguration configuration)
        {
            services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.Section))
                    .AddSingleton<IValidateOptions<CacheOptions>, CacheOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<CacheOptions>>().Value)
                    .AddOptionsWithValidateOnStart<CacheOptions>();

            services.AddMemoryCache()
                    .AddSingleton<ICache, Cache>();
        }

        private void AddChangelog(IConfiguration configuration)
        {
            services.Configure<ChangelogEndpointOptions>(configuration.GetSection(ChangelogEndpointOptions.Section))
                    .AddSingleton<IValidateOptions<ChangelogEndpointOptions>, ChangelogEndpointOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<ChangelogEndpointOptions>>().Value)
                    .AddOptionsWithValidateOnStart<ChangelogEndpointOptions>();

            services.AddHttpClient<IChangelogClient, ChangelogClient>();
        }

        private void AddCountries(IConfiguration configuration)
        {
            services.Configure<CountryEndpointOptions>(configuration.GetSection(CountryEndpointOptions.Section))
                    .AddSingleton<IValidateOptions<CountryEndpointOptions>, CountryEndpointOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<CountryEndpointOptions>>().Value)
                    .AddOptionsWithValidateOnStart<CountryEndpointOptions>();

            services.AddHttpClient<ICountryClient, CountryClient>();
            services.AddSingleton<ICountryRepository, CountryRepository>();
        }
    }
}
