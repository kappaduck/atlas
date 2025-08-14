// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Microsoft.Extensions.Localization;

namespace Atlas.Application.Countries;

internal static class CountryMappers
{
    extension(Country country)
    {
        internal CountryResponse ToResponse(IStringLocalizer<Resources> localizer)
        {
            (Uri map, Uri flag, _) = country.Resources;

            return new CountryResponse(country.Cca2, localizer[country.Cca2], new ResourcesResponse(map, flag));
        }
    }
}
