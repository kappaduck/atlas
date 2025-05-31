// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Caching;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        internal void AddCaching(IConfiguration configuration)
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
