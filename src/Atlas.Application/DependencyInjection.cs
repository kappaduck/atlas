// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Queries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddApplication()
        {
            services.AddChangelog();
            services.AddCountries();
        }

        private void AddChangelog() => services.AddSingleton<IGetChangelog, GetChangelog>();

        private void AddCountries()
        {
            services.AddSingleton<IGetAllCountries, GetAllCountries>();
            services.AddSingleton<IGetCountry, GetCountry>();
            services.AddSingleton<IGetDailyCountry, GetDailyCountry>();
            services.AddSingleton<IGetDailyFlag, GetDailyFlag>();
            services.AddSingleton<ILookupCountries, LookupCountries>();
            services.AddSingleton<IRandomizeCountry, RandomizeCountry>();
            services.AddSingleton<IGuessCountry, GuessCountry>();
        }
    }
}
