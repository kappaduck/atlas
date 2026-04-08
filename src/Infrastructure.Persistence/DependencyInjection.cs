// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Infrastructure.Persistence.Caching;
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
        public void AddPersistence(IConfiguration configuration) => services.AddCaching(configuration);

        private void AddCaching(IConfiguration configuration)
        {
            services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.Section))
                    .AddSingleton<IValidateOptions<CacheOptions>, CacheOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<CacheOptions>>().Value)
                    .AddOptionsWithValidateOnStart<CacheOptions>();

            services.AddMemoryCache()
                    .AddSingleton<ICache, Cache>();
        }
    }
}
