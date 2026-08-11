// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Globalization;
using Web.App.Settings;

namespace Web.App.Games.Components;

public partial class Guesses
{
    private readonly NumberFormatInfo _numberFormat = new()
    {
        NumberGroupSeparator = " "
    };

    [Parameter]
    public bool Flag { get; init; }

    [CascadingParameter]
    public required GameState GameState { get; init; }

    [CascadingParameter]
    public required AppState State { get; init; }

    private static string SuccessCss(bool success) => success ? "success" : string.Empty;

    private static string SameContinentCss(bool same) => same ? "quack-badge-accent" : "quack-badge-danger";
}
