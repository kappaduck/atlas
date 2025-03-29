// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prometheus.Files.Options;

namespace Prometheus.Files;

internal static class DependencyInjection
{
    internal static void AddFiles(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IDirectory, Directory>()
                        .AddTransient<IDataDirectory, DataDirectory>();

        builder.Services.Configure<DataPathOptions>(builder.Configuration.GetSection(DataPathOptions.Section))
                        .AddSingleton<IValidateOptions<DataPathOptions>, DataPathOptions.Validator>()
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<DataPathOptions>>().Value)
                        .AddOptionsWithValidateOnStart<DataPathOptions>();
    }
}
