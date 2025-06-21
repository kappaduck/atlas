// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Settings.Components;

public sealed partial class DifficultySettings
{
    [CascadingParameter]
    public required AppSettings Settings { get; set; }

    private bool FlagAllEnabled => Settings.Flag.All != Difficulty.None;

    private string FlagAllEnabledCss => FlagAllEnabled ? "disabled" : string.Empty;

    private void OnFlagAllDifficultyChange(Difficulty difficulty)
    {
        Settings.Flag = Settings.Flag with
        {
            All = difficulty,
            Daily = Difficulty.None,
            Random = Difficulty.None
        };
    }

    private void OnFlagDailyDifficultyChange(Difficulty difficulty)
    {
        Settings.Flag = Settings.Flag with
        {
            Daily = difficulty
        };
    }

    private void OnFlagRandomDifficultyChange(Difficulty difficulty)
    {
        Settings.Flag = Settings.Flag with
        {
            Random = difficulty
        };
    }
}
