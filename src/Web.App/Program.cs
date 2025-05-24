// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Web.App;
using Web.App.Settings;
using Web.App.Storage;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.ConfigureLoggings();
builder.AddOptions();
builder.AddServices();

await builder.Build()
             .UseLocalization()
             .RunAsync()
             .ConfigureAwait(false);

[ExcludeFromCodeCoverage]
file static partial class Program
{
    extension(WebAssemblyHost host)
    {
        internal WebAssemblyHost UseLocalization()
        {
            ILocalStorage storage = host.Services.GetRequiredService<ILocalStorage>();
            AppSettings.Data? data = storage.GetItem<AppSettings.Data>(LocalStorageKeys.Settings);

            if (data is null)
                return host;

            CultureInfo culture = data.Language switch
            {
                Language.English => new CultureInfo("en"),
                Language.French => new CultureInfo("fr-CA"),
                _ => new CultureInfo("en")
            };

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            return host;
        }
    }
}
