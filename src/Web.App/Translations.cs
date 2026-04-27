// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Web.App;

internal sealed class Translations
{
    private Translations()
    {
    }

    internal const string Atlas = nameof(Atlas);

    internal const string AtlasLogo = nameof(AtlasLogo);

    internal const string BackToGames = nameof(BackToGames);

    internal const string Blur = nameof(Blur);

    internal const string CountriesAvailable = nameof(CountriesAvailable);

    internal const string Country = nameof(Country);

    internal const string CountryGame = nameof(CountryGame);

    internal const string CountryModifiersCombined = nameof(CountryModifiersCombined);

    internal const string Daily = nameof(Daily);

    internal const string DailyPuzzleRandomPuzzle = nameof(DailyPuzzleRandomPuzzle);

    internal const string EndlessCountryGuessing = nameof(EndlessCountryGuessing);

    internal const string EndlessFlagGuessing = nameof(EndlessFlagGuessing);

    internal const string Flag = nameof(Flag);

    internal const string FlagGame = nameof(FlagGame);

    internal const string FlagModifiersMutuallyExclusive = nameof(FlagModifiersMutuallyExclusive);

    internal const string Grayscale = nameof(Grayscale);

    internal const string GuessMysteryCountry = nameof(GuessMysteryCountry);

    internal const string GuessMysteryFlag = nameof(GuessMysteryFlag);

    internal const string HueShift = nameof(HueShift);

    internal const string Invert = nameof(Invert);

    internal const string Mirror = nameof(Mirror);

    internal const string NoneDefault = nameof(NoneDefault);

    internal const string PageNotFound = nameof(PageNotFound);

    internal const string PageNotFoundMessage = nameof(PageNotFoundMessage);

    internal const string PickPreferedVisualModifier = nameof(PickPreferedVisualModifier);

    internal const string PickYourChallenge = nameof(PickYourChallenge);

    internal const string Random = nameof(Random);

    internal const string ReportBug = nameof(ReportBug);

    internal const string RequestFeature = nameof(RequestFeature);

    internal const string Rotate = nameof(Rotate);

    internal const string SearchCountries = nameof(SearchCountries);

    internal const string Settings = nameof(Settings);
}

internal static class TranslationsExtensions
{
    extension(IStringLocalizer<Translations> localizer)
    {
        internal string Atlas => localizer[Translations.Atlas];

        internal string AtlasLogo => localizer[Translations.AtlasLogo];

        internal string BackToGames => localizer[Translations.BackToGames];

        internal string Blur => localizer[Translations.Blur];

        internal string CountriesAvailable(int count) => localizer[Translations.CountriesAvailable, count];

        internal string Country => localizer[Translations.Country];

        internal string CountryGame => localizer[Translations.CountryGame];

        internal string CountryModifiersCombined => localizer[Translations.CountryModifiersCombined];

        internal string Daily => localizer[Translations.Daily];

        internal MarkupString DailyPuzzleRandomPuzzle => new(localizer[Translations.DailyPuzzleRandomPuzzle]);

        internal string EndlessCountryGuessing => localizer[Translations.EndlessCountryGuessing];

        internal string EndlessFlagGuessing => localizer[Translations.EndlessFlagGuessing];

        internal string Flag => localizer[Translations.Flag];

        internal string FlagGame => localizer[Translations.FlagGame];

        internal string FlagModifiersMutuallyExclusive => localizer[Translations.FlagModifiersMutuallyExclusive];

        internal string Grayscale => localizer[Translations.Grayscale];

        internal string GuessMysteryCountry => localizer[Translations.GuessMysteryCountry];

        internal string GuessMysteryFlag => localizer[Translations.GuessMysteryFlag];

        internal string Invert => localizer[Translations.Invert];

        internal string HueShift => localizer[Translations.HueShift];

        internal string Mirror => localizer[Translations.Mirror];

        internal string NoneDefault => localizer[Translations.NoneDefault];

        internal MarkupString PageNotFound => new(localizer[Translations.PageNotFound]);

        internal string PageNotFoundMessage => localizer[Translations.PageNotFoundMessage];

        internal string PickPreferedVisualModifier => localizer[Translations.PickPreferedVisualModifier];

        internal MarkupString PickYourChallenge => new(localizer[Translations.PickYourChallenge]);

        internal string Random => localizer[Translations.Random];

        internal string ReportBug => localizer[Translations.ReportBug];

        internal string RequestFeature => localizer[Translations.RequestFeature];

        internal string Rotate => localizer[Translations.Rotate];

        internal string SearchCountries => localizer[Translations.SearchCountries];

        internal string Settings => localizer[Translations.Settings];
    }
}
