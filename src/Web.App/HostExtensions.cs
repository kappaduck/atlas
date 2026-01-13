// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Web.App.Options;
using Web.App.Services;
using Web.App.Settings;
using Web.App.Storage;

namespace Web.App;

[ExcludeFromCodeCoverage]
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

            builder.Services.AddTransient<ITimeService, TimeService>();
            builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
            builder.Services.AddSingleton<IDailyLocalStorage, DailyLocalStorage>();
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

            builder.Services.Configure<CompanyOptions>(builder.Configuration.GetRequiredSection(CompanyOptions.Section))
                            .AddSingleton<IValidateOptions<CompanyOptions>, CompanyOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CompanyOptions>>().Value);

            builder.Services.Configure<FeatureOptions>(builder.Configuration.GetRequiredSection(FeatureOptions.Section))
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<FeatureOptions>>().Value);
        }
    }

    extension(WebAssemblyHost host)
    {
        internal void UseLocalization()
        {
            ILocalStorage storage = host.Services.GetRequiredService<ILocalStorage>();
            AppSettings.Data? data = storage.GetItem<AppSettings.Data>(LocalStorageKeys.Settings);

            if (data is null)
                return;

            CultureInfo culture = data.General.Language switch
            {
                Language.English => new CultureInfo("en"),
                Language.French => new CultureInfo("fr-CA"),
                _ => new CultureInfo("en")
            };

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
