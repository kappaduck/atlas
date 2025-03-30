// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Prometheus.Configurations;
using Prometheus.Countries;
using Prometheus.Files;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.ConfigureLoggings();
builder.ConfigureHttpClients();

builder.AddFiles();

builder.AddCountries();
builder.Services.AddHostedService<Application>();

await builder.Build().RunAsync().ConfigureAwait(false);
