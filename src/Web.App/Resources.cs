// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Web.App;

[ExcludeFromCodeCoverage]
internal sealed class Resources
{
    private Resources()
    {
    }

    internal const string Atlas = nameof(Atlas);

    internal const string AtlasLogo = nameof(AtlasLogo);

    internal const string Changelog = nameof(Changelog);

    internal const string ChangingLanguageWarning = nameof(ChangingLanguageWarning);

    internal const string CountryList = nameof(CountryList);

    internal const string Display = nameof(Display);

    internal const string GoBack = nameof(GoBack);

    internal const string Language = nameof(Language);

    internal const string LookupCountry = nameof(LookupCountry);

    internal const string LookupPlaceholder = nameof(LookupPlaceholder);

    internal const string Navigation = nameof(Navigation);

    internal const string NoCountriesAvailable = nameof(NoCountriesAvailable);

    internal const string NotFound = nameof(NotFound);

    internal const string NotFoundMessage = nameof(NotFoundMessage);

    internal const string Project = nameof(Project);

    internal const string Settings = nameof(Settings);

    internal const string Theme = nameof(Theme);
}
