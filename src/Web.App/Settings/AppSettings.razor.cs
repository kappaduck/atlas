// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using Web.App.Storage;

namespace Web.App.Settings;

public sealed partial class AppSettings(ILocalStorage storage, NavigationManager navigation)
{
    private Data _data = new();

    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }

    public Theme Theme
    {
        get => _data.General.Theme;
        set
        {
            if (_data.General.Theme == value)
                return;

            _data = _data with { General = _data.General with { Theme = value } };
            storage.SetItem(LocalStorageKeys.Settings, _data);

            ChangeTheme(value.ToString());
        }
    }

    public Language Language
    {
        get => _data.General.Language;
        set
        {
            if (_data.General.Language == value)
                return;

            _data = _data with { General = _data.General with { Language = value } };
            storage.SetItem(LocalStorageKeys.Settings, _data);

            navigation.Refresh();
        }
    }

    public FlagDifficulty Flag
    {
        get => _data.Flag;
        set
        {
            if (_data.Flag == value)
                return;

            _data = _data with { Flag = value };
            storage.SetItem(LocalStorageKeys.Settings, _data);

            StateHasChanged();
        }
    }

    public string DifficultyCss(Difficulty difficulty, int attempts)
    {
        if (Flag.All != Difficulty.None)
            return GetDifficulty(Flag.All);

        return GetDifficulty(difficulty);

        string GetDifficulty(Difficulty difficulty) => difficulty switch
        {
            Difficulty.Blur => $"blur-{attempts}",
            Difficulty.Invert => "invert",
            Difficulty.Shift => "shift",
            Difficulty.Grayscale => "grayscale",
            Difficulty.None => string.Empty
        };
    }

    [SupportedOSPlatform("browser")]
    protected override async Task OnInitializedAsync()
    {
        await JSHost.ImportAsync("settings", "/scripts/settings.js");

        _data = storage.GetItem<Data>(LocalStorageKeys.Settings) ?? _data;
        ChangeTheme(_data.General.Theme.ToString());
    }

    [JSImport("changeTheme", "settings")]
    private static partial void ChangeTheme(string theme);

    internal sealed record Data
    {
        public General General { get; init; } = new();

        public FlagDifficulty Flag { get; init; } = new();
    }
}
