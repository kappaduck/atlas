// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Languages;

namespace Atlas.Application.Countries;

internal static class CountryMappers
{
    extension(Country country)
    {
        internal CountryResponse ToResponse()
        {
            string name = country.Translations.First(t => t.Language == Language.English).Name;
            (Uri map, Uri flag, _) = country.Resources;

            return new CountryResponse(country.Cca2, name, new ResourcesResponse(map, flag));
        }
    }
}
