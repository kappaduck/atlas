// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;
using Infrastructure.Persistence.Changelog.Sources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Changelog;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        internal void AddChangelog(IConfiguration configuration)
        {
            services.Configure<ChangelogOptions>(configuration.GetSection(ChangelogOptions.Section))
                    .AddSingleton<IValidateOptions<ChangelogOptions>, ChangelogOptions.Validator>()
                    .AddSingleton(sp => sp.GetRequiredService<IOptions<ChangelogOptions>>().Value)
                    .AddOptionsWithValidateOnStart<ChangelogOptions>();

            services.AddHttpClient<IChangelogClient, ChangelogClient>();
            services.AddSingleton<IChangelogRepository, ChangelogRepository>();
        }
    }
}
