// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using Web.App.Options;
using Web.App.Services;
using Web.App.Storage;

namespace Web.App;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(WebAssemblyHostBuilder builder)
    {
        internal void AddOptions()
        {
            builder.Services.Configure<ProjectOptions>(builder.Configuration.GetRequiredSection(ProjectOptions.Section))
                            .AddSingleton<IValidateOptions<ProjectOptions>, ProjectOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<ProjectOptions>>().Value);

            builder.Services.Configure<CompanyOptions>(builder.Configuration.GetRequiredSection(CompanyOptions.Section))
                            .AddSingleton<IValidateOptions<CompanyOptions>, CompanyOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CompanyOptions>>().Value);

            builder.Services.Configure<FeatureOptions>(builder.Configuration.GetRequiredSection(FeatureOptions.Section))
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<FeatureOptions>>().Value);
        }

        internal void AddServices()
        {
            builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());
            builder.Services.AddLocalization();

            builder.Services.AddTransient<ITimeService, TimeService>();
            builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
        }

        internal void ConfigureLoggings()
        {
            if (!builder.HostEnvironment.IsProduction())
                return;

            builder.Logging.ClearProviders();
        }
    }
}
