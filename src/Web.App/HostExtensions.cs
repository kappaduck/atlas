// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Web.App.Options;
using Web.App.Storage;

namespace Web.App;

internal static class HostExtensions
{
    extension(WebAssemblyHostBuilder builder)
    {
        internal void AddServices()
        {
            builder.UseDefaultServiceProvider((env, options) =>
            {
                options.ValidateOnBuild = true;
                options.ValidateScopes = env.IsDevelopment();
            });

            builder.ConfigureOptions();

            builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());
            builder.Services.AddLocalization();

            builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
            builder.Services.AddKeyedSingleton<IDailyLocalStorage, DailyFlagStorage>(DailyFlagStorage.Key);
            builder.Services.AddKeyedSingleton<IDailyLocalStorage, DailyCountryStorage>(DailyCountryStorage.Key);
        }

        internal void ConfigureLoggings()
        {
            if (!builder.HostEnvironment.IsProduction())
                return;

            builder.Logging.ClearProviders();
        }

        private void ConfigureOptions()
        {
            builder.Services.Configure<ProjectOptions>(builder.Configuration.GetRequiredSection(ProjectOptions.Section))
                            .AddSingleton<IValidateOptions<ProjectOptions>, ProjectOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<ProjectOptions>>().Value);

            builder.Services.Configure<DevOptions>(builder.Configuration.GetRequiredSection(DevOptions.Section))
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<DevOptions>>().Value);
        }
    }
}
