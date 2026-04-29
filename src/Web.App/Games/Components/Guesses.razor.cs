// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Globalization;
using Web.App.Settings;

namespace Web.App.Games.Components;

public sealed partial class Guesses
{
    private readonly NumberFormatInfo _numberFormat = new()
    {
        NumberGroupSeparator = " "
    };

    [Parameter]
    public bool FlagHint { get; init; }

    [CascadingParameter]
    public required GameState GameState { get; init; }

    [CascadingParameter]
    public required AppState State { get; init; }

    private static string SuccessCss(bool success) => success ? "success" : "wrong";

    private static string SameContinentCss(bool same) => same ? "same" : string.Empty;
}
