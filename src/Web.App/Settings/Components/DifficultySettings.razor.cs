// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Settings.Components;

public sealed partial class DifficultySettings
{
    [CascadingParameter]
    public required AppSettings Settings { get; set; }

    private bool FlagAllEnabled => Settings.Flag.All != FlagDifficulty.None;

    private bool CountryAllEnabled => Settings.Country.All != CountryDifficulty.None;

    private string FlagAllEnabledCss => FlagAllEnabled ? "disabled" : string.Empty;

    private string CountryAllEnabledCss => CountryAllEnabled ? "disabled" : string.Empty;

    private void OnFlagAllDifficultyChange(FlagDifficulty difficulty) => Settings.Flag = new FlagDifficultySettings
    {
        All = difficulty,
        Daily = FlagDifficulty.None,
        Random = FlagDifficulty.None
    };

    private void OnFlagDailyDifficultyChange(FlagDifficulty difficulty)
    {
        Settings.Flag = Settings.Flag with
        {
            Daily = difficulty
        };
    }

    private void OnFlagRandomDifficultyChange(FlagDifficulty difficulty)
    {
        Settings.Flag = Settings.Flag with
        {
            Random = difficulty
        };
    }

    private void OnCountryAllDifficultyChange(CountryDifficulty difficulty) => Settings.Country = new CountryDifficultySettings
    {
        All = difficulty,
        Daily = CountryDifficulty.None,
        Random = CountryDifficulty.None
    };

    private void OnCountryDailyDifficultyChange(CountryDifficulty difficulty)
    {
        Settings.Country = Settings.Country with
        {
            Daily = difficulty
        };
    }

    private void OnCountryRandomDifficultyChange(CountryDifficulty difficulty)
    {
        Settings.Country = Settings.Country with
        {
            Random = difficulty
        };
    }
}
