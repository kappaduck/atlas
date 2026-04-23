// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace Web.App;

[ExcludeFromCodeCoverage]
internal sealed class Translations
{
    private Translations()
    {
    }

    internal const string Atlas = nameof(Atlas);

    internal const string AtlasLogo = nameof(AtlasLogo);

    internal const string Country = nameof(Country);

    internal const string Daily = nameof(Daily);

    internal const string DailyPuzzleRandomPuzzle = nameof(DailyPuzzleRandomPuzzle);

    internal const string EndlessCountryGuessing = nameof(EndlessCountryGuessing);

    internal const string EndlessFlagGuessing = nameof(EndlessFlagGuessing);

    internal const string Flag = nameof(Flag);

    internal const string GuessMysteryCountry = nameof(GuessMysteryCountry);

    internal const string GuessMysteryFlag = nameof(GuessMysteryFlag);

    internal const string PickYourChallenge = nameof(PickYourChallenge);

    internal const string Random = nameof(Random);

    internal const string ReportBug = nameof(ReportBug);

    internal const string RequestFeature = nameof(RequestFeature);
}

internal static class TranslationsExtensions
{
    extension(IStringLocalizer<Translations> localizer)
    {
        internal string Atlas => localizer[Translations.Atlas];

        internal string AtlasLogo => localizer[Translations.AtlasLogo];

        internal string Country => localizer[Translations.Country];

        internal string Daily => localizer[Translations.Daily];

        internal MarkupString DailyPuzzleRandomPuzzle => new(localizer[Translations.DailyPuzzleRandomPuzzle]);

        internal string EndlessCountryGuessing => localizer[Translations.EndlessCountryGuessing];

        internal string EndlessFlagGuessing => localizer[Translations.EndlessFlagGuessing];

        internal string Flag => localizer[Translations.Flag];

        internal string GuessMysteryCountry => localizer[Translations.GuessMysteryCountry];

        internal string GuessMysteryFlag => localizer[Translations.GuessMysteryFlag];

        internal MarkupString PickYourChallenge => new(localizer[Translations.PickYourChallenge]);

        internal string Random => localizer[Translations.Random];

        internal string ReportBug => localizer[Translations.ReportBug];

        internal string RequestFeature => localizer[Translations.RequestFeature];
    }
}
