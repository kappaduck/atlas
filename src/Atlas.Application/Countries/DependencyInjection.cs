// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application.Countries;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        internal void AddCountries()
        {
            services.AddSingleton<IGetAllCountries, GetAllCountries>();
            services.AddSingleton<IGetCountry, GetCountry>();
            services.AddSingleton<IGetDailyCountry, GetDailyCountry>();
            services.AddSingleton<ILookupCountries, LookupCountries>();
            services.AddSingleton<IRandomizeCountry, RandomizeCountry>();
            services.AddSingleton<IGuessCountry, GuessCountry>();
        }
    }
}
