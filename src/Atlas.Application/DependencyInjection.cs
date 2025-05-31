// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddApplication() => services.AddMediator(options => options.GenerateTypesAsInternal = true);
    }
}
