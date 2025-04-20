// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using Web.App.Services;

namespace Web.App;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(WebAssemblyHostBuilder builder)
    {
        internal void AddServices()
        {
            builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());

            builder.Services.AddTransient<ITimeService, TimeService>();
        }

        internal void ConfigureLoggings()
        {
            if (!builder.HostEnvironment.IsProduction())
                return;

            builder.Logging.ClearProviders();
        }
    }
}
