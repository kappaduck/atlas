// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Settings.Components;

public sealed partial class InputDifficulty
{
    [Parameter]
    public FlagDifficulty Value { get; init; }

    [Parameter]
    public EventCallback<FlagDifficulty> ValueChanged { get; init; }

    [Parameter, EditorRequired]
    public required string GroupName { get; init; }

    [Parameter]
    public bool Disabled { get; init; }

    private static (FlagDifficulty Difficulty, string Name)[] GetDifficulties()
        => [.. Enum.GetValues<FlagDifficulty>().Select(d => (d, d.ToString()))];

    private string GetRadioId(string difficultyName)
        => $"{GroupName}-{difficultyName}";
}
