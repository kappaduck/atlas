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

    internal const string Appearance = nameof(Appearance);

    internal const string Atlas = nameof(Atlas);

    internal const string AtlasLogo = nameof(AtlasLogo);

    internal const string BackToGames = nameof(BackToGames);

    internal const string Blur = nameof(Blur);

    internal const string Changelog = nameof(Changelog);

    internal const string ChooseDisplayLanguage = nameof(ChooseDisplayLanguage);

    internal const string ChooseDistanceUnit = nameof(ChooseDistanceUnit);

    internal const string ChooseTheme = nameof(ChooseTheme);

    internal const string ColourCodeContinentMatch = nameof(ColourCodeContinentMatch);

    internal const string Countries = nameof(Countries);

    internal const string CountriesAvailable = nameof(CountriesAvailable);

    internal const string Country = nameof(Country);

    internal const string CountryGame = nameof(CountryGame);

    internal const string CountryModifiersCombined = nameof(CountryModifiersCombined);

    internal const string Daily = nameof(Daily);

    internal const string DailyPuzzleRandomPuzzle = nameof(DailyPuzzleRandomPuzzle);

    internal const string Difficulty = nameof(Difficulty);

    internal const string DisplayDirectionalArrow = nameof(DisplayDirectionalArrow);

    internal const string DisplayDistance = nameof(DisplayDistance);

    internal const string Distance = nameof(Distance);

    internal const string EndlessCountryGuessing = nameof(EndlessCountryGuessing);

    internal const string EndlessFlagGuessing = nameof(EndlessFlagGuessing);

    internal const string Flag = nameof(Flag);

    internal const string FlagGame = nameof(FlagGame);

    internal const string FlagModifiersMutuallyExclusive = nameof(FlagModifiersMutuallyExclusive);

    internal const string General = nameof(General);

    internal const string Grayscale = nameof(Grayscale);

    internal const string GuessMysteryCountry = nameof(GuessMysteryCountry);

    internal const string GuessMysteryFlag = nameof(GuessMysteryFlag);

    internal const string Hints = nameof(Hints);

    internal const string HueShift = nameof(HueShift);

    internal const string Invert = nameof(Invert);

    internal const string Language = nameof(Language);

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

    internal const string ShowContinentHint = nameof(ShowContinentHint);

    internal const string ShowDistance = nameof(ShowDistance);

    internal const string ShowDirectionArrow = nameof(ShowDirectionArrow);

    internal const string Theme = nameof(Theme);
}

internal static class TranslationsExtensions
{
    extension(IStringLocalizer<Translations> localizer)
    {
        internal string Appearance => localizer[Translations.Appearance];

        internal string Atlas => localizer[Translations.Atlas];

        internal string AtlasLogo => localizer[Translations.AtlasLogo];

        internal string BackToGames => localizer[Translations.BackToGames];

        internal string Blur => localizer[Translations.Blur];

        internal string Changelog => localizer[Translations.Changelog];

        internal string ChooseDisplayLanguage => localizer[Translations.ChooseDisplayLanguage];

        internal string ChooseDistanceUnit => localizer[Translations.ChooseDistanceUnit];

        internal string ChooseTheme => localizer[Translations.ChooseTheme];

        internal string ColourCodeContinentMatch => localizer[Translations.ColourCodeContinentMatch];

        internal string Countries => localizer[Translations.Countries];

        internal string CountriesAvailable(int count) => localizer[Translations.CountriesAvailable, count];

        internal string Country => localizer[Translations.Country];

        internal string CountryGame => localizer[Translations.CountryGame];

        internal string CountryModifiersCombined => localizer[Translations.CountryModifiersCombined];

        internal string Daily => localizer[Translations.Daily];

        internal MarkupString DailyPuzzleRandomPuzzle => new(localizer[Translations.DailyPuzzleRandomPuzzle]);

        internal string Difficulty => localizer[Translations.Difficulty];

        internal string DisplayDirectionalArrow => localizer[Translations.DisplayDirectionalArrow];

        internal string DisplayDistance => localizer[Translations.DisplayDistance];

        internal string Distance => localizer[Translations.Distance];

        internal string EndlessCountryGuessing => localizer[Translations.EndlessCountryGuessing];

        internal string EndlessFlagGuessing => localizer[Translations.EndlessFlagGuessing];

        internal string Flag => localizer[Translations.Flag];

        internal string FlagGame => localizer[Translations.FlagGame];

        internal string FlagModifiersMutuallyExclusive => localizer[Translations.FlagModifiersMutuallyExclusive];

        internal string General => localizer[Translations.General];

        internal string Grayscale => localizer[Translations.Grayscale];

        internal string GuessMysteryCountry => localizer[Translations.GuessMysteryCountry];

        internal string GuessMysteryFlag => localizer[Translations.GuessMysteryFlag];

        internal string Invert => localizer[Translations.Invert];

        internal string Hints => localizer[Translations.Hints];

        internal string HueShift => localizer[Translations.HueShift];

        internal string Language => localizer[Translations.Language];

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

        internal string ShowContinentHint => localizer[Translations.ShowContinentHint];

        internal string ShowDistance => localizer[Translations.ShowDistance];

        internal string ShowDirectionArrow => localizer[Translations.ShowDirectionArrow];

        internal string Theme => localizer[Translations.Theme];
    }
}
