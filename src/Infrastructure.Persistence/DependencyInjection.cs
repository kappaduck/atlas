// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;
using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Changelog;
using Infrastructure.Persistence.Changelog.Sources;
using Infrastructure.Persistence.Countries;
using Infrastructure.Persistence.Countries.Options;
using Infrastructure.Persistence.Countries.Sources;
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
        public void AddInfrastructure(IConfiguration configuration, Action<HttpClient> configure)
        {
            services.AddCaching(configuration);
            services.AddCountries(configure, configuration);
            services.AddChangelog(configuration);
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
            services.Configure<ChangelogOptions>(configuration.GetSection(ChangelogOptions.Section))
                    .AddSingleton<IValidateOptions<ChangelogOptions>, ChangelogOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<ChangelogOptions>>().Value)
                    .AddOptionsWithValidateOnStart<ChangelogOptions>();

            services.AddHttpClient<IChangelogClient, ChangelogClient>();
            services.AddSingleton<IChangelogRepository, ChangelogRepository>();
        }

        private void AddCountries(Action<HttpClient> configure, IConfiguration configuration)
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
