// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using Web.App.Storage;

namespace Web.App.Settings;

public sealed partial class CascadingSettings(ILocalStorage storage, NavigationManager navigation)
{
    private AppSettings _settings = new();

    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }

    [SupportedOSPlatform("browser")]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSHost.ImportAsync("settings", "/scripts/settings.js");

            _settings = storage.GetItem<AppSettings>(LocalStorageKeys.Settings) ?? _settings;
            ChangeTheme(_settings.Theme.ToString());
        }
    }

    internal void ChangeTheme(Theme theme)
    {
        _settings = _settings with { Theme = theme };
        storage.SetItem(LocalStorageKeys.Settings, _settings);

        ChangeTheme(theme.ToString());
        StateHasChanged();
    }

    internal void ChangeLanguage(Language language)
    {
        _settings = _settings with { Language = language };
        storage.SetItem(LocalStorageKeys.Settings, _settings);

        navigation.Refresh();
    }

    [JSImport("changeTheme", "settings")]
    private static partial void ChangeTheme(string theme);
}
