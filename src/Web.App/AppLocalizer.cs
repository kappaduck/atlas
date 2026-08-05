// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Localization;

namespace Web.App;

internal sealed class AppLocalizer(IStringLocalizer<AppLocalizer> localizer)
{
    internal string Atlas { get; } = localizer[nameof(Atlas)];

    internal string AtlasDescription { get; } = localizer[nameof(AtlasDescription)];

    internal string AtlasGitHub { get; } = localizer[nameof(AtlasGitHub)];

    internal string BrowseIssues { get; } = localizer[nameof(BrowseIssues)];

    internal string BrowseModes { get; } = localizer[nameof(BrowseModes)];

    internal string ChooseGame { get; } = localizer[nameof(ChooseGame)];

    internal string ComingSoon { get; } = localizer[nameof(ComingSoon)];

    internal string Daily { get; } = localizer[nameof(Daily)];

    internal string DailyCountry { get; } = localizer[nameof(DailyCountry)];

    internal string DailyCountryDescription { get; } = localizer[nameof(DailyCountryDescription)];

    internal string DailyFlag { get; } = localizer[nameof(DailyFlag)];

    internal string DailyFlagDescription { get; } = localizer[nameof(DailyFlagDescription)];

    internal string DailyGeographyGame { get; } = localizer[nameof(DailyGeographyGame)];

    internal string Endless { get; } = localizer[nameof(Endless)];

    internal string FlagQuiz { get; } = localizer[nameof(FlagQuiz)];

    internal string FlagQuizDescription { get; } = localizer[nameof(FlagQuizDescription)];

    internal string GuessCountry { get; } = localizer[nameof(GuessCountry)];

    internal string HideSeek { get; } = localizer[nameof(HideSeek)];

    internal string HideSeekDescription { get; } = localizer[nameof(HideSeekDescription)];

    internal string Home { get; } = localizer[nameof(Home)];

    internal string Logo { get; } = localizer[nameof(Logo)];

    internal string ModeDescription { get; } = localizer[nameof(ModeDescription)];

    internal string OpenIssue { get; } = localizer[nameof(OpenIssue)];

    internal string PixelatedFlag { get; } = localizer[nameof(PixelatedFlag)];

    internal string PixelatedFlagDescription { get; } = localizer[nameof(PixelatedFlagDescription)];

    internal string Play { get; } = localizer[nameof(Play)];

    internal string PlayTodayFlag { get; } = localizer[nameof(PlayTodayFlag)];

    internal string ReportBug { get; } = localizer[nameof(ReportBug)];

    internal string RequestFeature { get; } = localizer[nameof(RequestFeature)];

    internal string ReverseCountry { get; } = localizer[nameof(ReverseCountry)];

    internal string ReverseCountryDescription { get; } = localizer[nameof(ReverseCountryDescription)];

    internal string SendFeedback { get; } = localizer[nameof(SendFeedback)];

    internal string Settings { get; } = localizer[nameof(Settings)];

    internal string UnlimitedCountry { get; } = localizer[nameof(UnlimitedCountry)];

    internal string UnlimitedCountryDescription { get; } = localizer[nameof(UnlimitedCountryDescription)];

    internal string UnlimitedFlag { get; } = localizer[nameof(UnlimitedFlag)];

    internal string UnlimitedFlagDescription { get; } = localizer[nameof(UnlimitedFlagDescription)];

    internal string ViewChangelog { get; } = localizer[nameof(ViewChangelog)];
}
