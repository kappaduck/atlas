// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Prometheus.Configurations;

[ExcludeFromCodeCoverage]
internal static class HttpClientConfigurations
{
    internal static void ConfigureHttpClients(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
            return;

        builder.Services.ConfigureHttpClientDefaults(b => b.RemoveAllLoggers());
    }
}
