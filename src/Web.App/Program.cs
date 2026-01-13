// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web.App;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.ConfigureLoggings();
builder.AddServices();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

WebAssemblyHost host = builder.Build();

host.UseLocalization();
await host.RunAsync();
