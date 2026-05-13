// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Games.Components;

public sealed partial class GameOver
{
    [CascadingParameter]
    public required GameState GameState { get; init; }

    [Parameter, EditorRequired]
    public required string SecondaryActionLabel { get; set; }

    [Parameter, EditorRequired]
    public required EventCallback OnSecondaryActionClick { get; set; }
}
