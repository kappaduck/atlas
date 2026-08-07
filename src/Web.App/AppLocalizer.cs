// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Localization;

namespace Web.App;

public sealed class AppLocalizer(IStringLocalizer<AppLocalizer> localizer)
{
    internal string Appearance { get; } = localizer[nameof(Appearance)];

    internal string Atlas { get; } = localizer[nameof(Atlas)];

    internal string AtlasDescription { get; } = localizer[nameof(AtlasDescription)];

    internal string AtlasGitHub { get; } = localizer[nameof(AtlasGitHub)];

    internal string Auto { get; } = localizer[nameof(Auto)];

    internal string BackToHome { get; } = localizer[nameof(BackToHome)];

    internal string Blur { get; } = localizer[nameof(Blur)];

    internal string BlurDescription { get; } = localizer[nameof(BlurDescription)];

    internal string BrowseIssues { get; } = localizer[nameof(BrowseIssues)];

    internal string BrowseModes { get; } = localizer[nameof(BrowseModes)];

    internal string Changelog { get; } = localizer[nameof(Changelog)];

    internal string ChooseGame { get; } = localizer[nameof(ChooseGame)];

    internal string ClearData { get; } = localizer[nameof(ClearData)];

    internal string ClearPreferences { get; } = localizer[nameof(ClearPreferences)];

    internal string Close { get; } = localizer[nameof(Close)];

    internal string ComingSoon { get; } = localizer[nameof(ComingSoon)];

    public string ConfirmClearData { get; } = localizer[nameof(ConfirmClearData)];

    internal string ContinentHint { get; } = localizer[nameof(ContinentHint)];

    internal string Countries { get; } = localizer[nameof(Countries)];

    internal string CountryBlurDescription { get; } = localizer[nameof(CountryBlurDescription)];

    internal string CountryDifficulty { get; } = localizer[nameof(CountryDifficulty)];

    internal string CountryDifficultyDescription { get; } = localizer[nameof(CountryDifficultyDescription)];

    internal string Daily { get; } = localizer[nameof(Daily)];

    internal string DailyCountry { get; } = localizer[nameof(DailyCountry)];

    internal string DailyCountryDescription { get; } = localizer[nameof(DailyCountryDescription)];

    internal string DailyFlag { get; } = localizer[nameof(DailyFlag)];

    internal string DailyFlagDescription { get; } = localizer[nameof(DailyFlagDescription)];

    internal string DailyGeographyGame { get; } = localizer[nameof(DailyGeographyGame)];

    internal string Data { get; } = localizer[nameof(Data)];

    internal string Difficulty { get; } = localizer[nameof(Difficulty)];

    internal string DirectionHint { get; } = localizer[nameof(DirectionHint)];

    internal string DistanceHint { get; } = localizer[nameof(DistanceHint)];

    internal string DistanceUnit { get; } = localizer[nameof(DistanceUnit)];

    internal string DistanceUnitDescription { get; } = localizer[nameof(DistanceUnitDescription)];

    internal string Endless { get; } = localizer[nameof(Endless)];

    internal string FlagDifficulty { get; } = localizer[nameof(FlagDifficulty)];

    internal string FlagDifficultyDescription { get; } = localizer[nameof(FlagDifficultyDescription)];

    internal string FlagHint { get; } = localizer[nameof(FlagHint)];

    internal string FlagHintDescription { get; } = localizer[nameof(FlagHintDescription)];

    internal string FlagQuiz { get; } = localizer[nameof(FlagQuiz)];

    internal string FlagQuizDescription { get; } = localizer[nameof(FlagQuizDescription)];

    internal string General { get; } = localizer[nameof(General)];

    internal string Grayscale { get; } = localizer[nameof(Grayscale)];

    internal string GrayscaleDescription { get; } = localizer[nameof(GrayscaleDescription)];

    internal string GuessCountry { get; } = localizer[nameof(GuessCountry)];

    internal string GuessFlagHint { get; } = localizer[nameof(GuessFlagHint)];

    internal string GuessFlagHintDescription { get; } = localizer[nameof(GuessFlagHintDescription)];

    internal string HideSeek { get; } = localizer[nameof(HideSeek)];

    internal string HideSeekDescription { get; } = localizer[nameof(HideSeekDescription)];

    internal string Hints { get; } = localizer[nameof(Hints)];

    internal string Home { get; } = localizer[nameof(Home)];

    internal string HueShift { get; } = localizer[nameof(HueShift)];

    internal string HueShiftDescription { get; } = localizer[nameof(HueShiftDescription)];

    internal string Invert { get; } = localizer[nameof(Invert)];

    internal string InvertDescription { get; } = localizer[nameof(InvertDescription)];

    internal string Kilometers { get; } = localizer[nameof(Kilometers)];

    internal string Language { get; } = localizer[nameof(Language)];

    internal string LanguageDescription { get; } = localizer[nameof(LanguageDescription)];

    internal string Logo { get; } = localizer[nameof(Logo)];

    internal string LostAtSea { get; } = localizer[nameof(LostAtSea)];

    internal string Miles { get; } = localizer[nameof(Miles)];

    internal string Mirror { get; } = localizer[nameof(Mirror)];

    internal string MirrorDescription { get; } = localizer[nameof(MirrorDescription)];

    internal string ModeDescription { get; } = localizer[nameof(ModeDescription)];

    internal string None { get; } = localizer[nameof(None)];

    internal string NoneDescription { get; } = localizer[nameof(NoneDescription)];

    internal string NotFoundMessage { get; } = localizer[nameof(NotFoundMessage)];

    internal string OpenIssue { get; } = localizer[nameof(OpenIssue)];

    internal string PixelatedFlag { get; } = localizer[nameof(PixelatedFlag)];

    internal string PixelatedFlagDescription { get; } = localizer[nameof(PixelatedFlagDescription)];

    internal string PlaceNotOnMap { get; } = localizer[nameof(PlaceNotOnMap)];

    internal string Play { get; } = localizer[nameof(Play)];

    internal string PlayTodayFlag { get; } = localizer[nameof(PlayTodayFlag)];

    internal string ProximityBarHint { get; } = localizer[nameof(ProximityBarHint)];

    internal string ReportBug { get; } = localizer[nameof(ReportBug)];

    internal string RequestFeature { get; } = localizer[nameof(RequestFeature)];

    internal string ResetAtlas { get; } = localizer[nameof(ResetAtlas)];

    internal string ReverseCountry { get; } = localizer[nameof(ReverseCountry)];

    internal string ReverseCountryDescription { get; } = localizer[nameof(ReverseCountryDescription)];

    internal string Rotate { get; } = localizer[nameof(Rotate)];

    internal string RotationDescription { get; } = localizer[nameof(RotationDescription)];

    internal string RotationRandomisedDescription { get; } = localizer[nameof(RotationRandomisedDescription)];

    internal string SendFeedback { get; } = localizer[nameof(SendFeedback)];

    internal string Settings { get; } = localizer[nameof(Settings)];

    internal string Theme { get; } = localizer[nameof(Theme)];

    internal string ThemeDescription { get; } = localizer[nameof(ThemeDescription)];

    internal string UnlimitedCountry { get; } = localizer[nameof(UnlimitedCountry)];

    internal string UnlimitedCountryDescription { get; } = localizer[nameof(UnlimitedCountryDescription)];

    internal string UnlimitedFlag { get; } = localizer[nameof(UnlimitedFlag)];

    internal string UnlimitedFlagDescription { get; } = localizer[nameof(UnlimitedFlagDescription)];

    internal string ViewChangelog { get; } = localizer[nameof(ViewChangelog)];
}
