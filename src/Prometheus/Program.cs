// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Hosting;
using Prometheus.Configurations;
using Prometheus.Files;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.ConfigureLoggings();

builder.AddFiles();

await builder.Build().RunAsync().ConfigureAwait(false);
