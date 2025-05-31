// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddApplication()
            => services.AddMediator(options => options.GenerateTypesAsInternal = true);
    }
}
