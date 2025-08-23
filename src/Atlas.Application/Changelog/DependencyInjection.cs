// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application.Changelog;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        internal void AddChangelog() => services.AddSingleton<IGetChangelog, GetChangelog>();
    }
}
