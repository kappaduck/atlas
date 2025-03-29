// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Hosting;
using Prometheus.Configurations;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.ConfigureLoggings();

await builder.Build().RunAsync().ConfigureAwait(false);
