// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Infrastructure.Persistence.Caching;
using Infrastructure.Persistence.Changelog;
using Infrastructure.Persistence.Countries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddCountries(configure);
            services.AddChangelog(configuration);
        }
    }
}
