// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Atlas.Application.Countries;

internal static class CountryMappers
{
    extension(Country country)
    {
        internal CountryResponse ToResponse()
        {
            string name = country.Translations.First(t => t.Language == Language.English).Name;
            (Uri map, Image flag) = country.Resource;

            return new CountryResponse(country.Cca2, name, new CountryResourceResponse(map, new ImageResponse(flag.Uri, flag.MediaType)));
        }
    }
}
