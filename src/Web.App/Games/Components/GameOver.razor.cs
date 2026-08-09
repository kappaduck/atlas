// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Games.Components;

public partial class GameOver
{
    private bool _previewEnabled;

    [CascadingParameter]
    public GameState GameState { get; init; }

    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }

    [Parameter]
    public string? Message { get; init; }

    [Parameter]
    public bool Preview { get; init; }

    [Parameter]
    public EventCallback<bool> OnPreviewClick { get; init; }

    private Task PreviewClickAsync()
    {
        _previewEnabled = !_previewEnabled;
        return OnPreviewClick.InvokeAsync(_previewEnabled);
    }
}
