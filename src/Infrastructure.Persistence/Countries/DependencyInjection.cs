// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Infrastructure.Persistence.Countries.Sources;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Countries;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        internal void AddCountries(Action<HttpClient> configure)
        {
            services.AddHttpClient<IDataSource<Country>, CountryDataSource>(configure);
            services.AddHttpClient<IDataSource<CountryLookup>, CountryLookupDataSource>(configure);

            services.AddSingleton<ICountryRepository, CountryRepository>();
            services.AddSingleton<ICountryLookupRepository, CountryLookupRepository>();
        }
    }
}
