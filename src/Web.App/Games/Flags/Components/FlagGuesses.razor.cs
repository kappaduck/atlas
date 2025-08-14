// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Globalization;
using Web.App.Components;
using Web.App.Components.Modals;

namespace Web.App.Games.Flags.Components;

public sealed partial class FlagGuesses
{
    private readonly NumberFormatInfo _numberFormat = new()
    {
        NumberGroupSeparator = " "
    };

    [CascadingParameter]
    public required GameState GameState { get; init; }

    [CascadingParameter]
    public required ZoomModal ZoomModal { get; init; }

    private string HasWonGameCss => GameState.GameFinished ? "found" : string.Empty;

    private static string FoundCountryCss(bool isFound) => isFound ? "found" : string.Empty;

    private static string DirectionCss(bool isFound) => isFound ? $"found {Icons.Check}" : Icons.ArrowUp;

    private static string IsSameContinentCss(bool IsSameContinent) => IsSameContinent ? $"found {Icons.Check}" : Icons.Times;
}
