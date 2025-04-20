// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Configurations;

[ExcludeFromCodeCoverage]
internal static class HttpClientConfigurations
{
    extension(IHostApplicationBuilder builder)
    {
        internal void ConfigureHttpClients()
        {
            if (builder.Environment.IsDevelopment())
                return;

            builder.Services.ConfigureHttpClientDefaults(b => b.RemoveAllLoggers());
        }
    }
}
