// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Globalization;
using Web.App.Components;

namespace Web.App.Games.Countries.Components;

public sealed partial class CountryGuesses
{
    private readonly NumberFormatInfo _numberFormat = new()
    {
        NumberGroupSeparator = " "
    };

    [CascadingParameter]
    public required GameState GameState { get; init; }

    private string HasWonGameCss => GameState.GameFinished ? "found" : string.Empty;

    private static string FoundCountryCss(bool isFound) => isFound ? "found" : string.Empty;

    private static string DirectionCss(bool isFound) => isFound ? $"found {Icons.Check}" : Icons.ArrowUp;

    private static string IsSameContinentCss(bool isSameContinent) => isSameContinent ? $"found {Icons.Check}" : Icons.Times;
}
