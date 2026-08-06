// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices.JavaScript;
using Web.App.Storage;

namespace Web.App.Settings;

public sealed partial class AppState(ILocalStorage storage, IJSInProcessRuntime jsRuntime, NavigationManager navigation)
{
    private const string StorageKey = "settings";
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
            storage.SetItem(StorageKey, _data);

            ChangeTheme(value);
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
            storage.SetItem(StorageKey, _data);

            navigation.Refresh();
        }
    }

    public DistanceUnit Unit
    {
        get => _data.General.Unit;
        set
        {
            if (_data.General.Unit == value)
                return;

            _data = _data with { General = _data.General with { Unit = value } };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public bool DistanceHint
    {
        get => _data.General.DistanceHint;
        set
        {
            if (_data.General.DistanceHint == value)
                return;

            _data = _data with { General = _data.General with { DistanceHint = value } };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public bool ArrowHint
    {
        get => _data.General.ArrowHint;
        set
        {
            if (_data.General.ArrowHint == value)
                return;

            _data = _data with { General = _data.General with { ArrowHint = value } };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public bool ContinentHint
    {
        get => _data.General.ContinentHint;
        set
        {
            if (_data.General.ContinentHint == value)
                return;

            _data = _data with { General = _data.General with { ContinentHint = value } };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public bool ProximityBarHint
    {
        get => _data.General.ProximityBarHint;
        set
        {
            if (_data.General.ProximityBarHint == value)
                return;

            _data = _data with { General = _data.General with { ProximityBarHint = value } };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public bool FlagHint
    {
        get => _data.General.FlagHint;
        set
        {
            if (_data.General.FlagHint == value)
                return;

            _data = _data with { General = _data.General with { FlagHint = value } };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public bool GuessFlagHint
    {
        get => _data.General.GuessFlagHint;
        set
        {
            if (_data.General.GuessFlagHint == value)
                return;

            _data = _data with { General = _data.General with { GuessFlagHint = value } };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
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
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public CountryDifficulty Country
    {
        get => _data.Country;
        set
        {
            if (_data.Country == value)
                return;

            _data = _data with { Country = value };
            storage.SetItem(StorageKey, _data);

            StateHasChanged();
        }
    }

    public void Reset()
    {
        Language oldLanguage = Language;

        Clear();
        _data = new Data();

        storage.SetItem(StorageKey, _data);

        if (oldLanguage != _data.General.Language)
            navigation.Refresh();
        else
            StateHasChanged();
    }

    protected override void OnInitialized()
    {
        try
        {
            _data = storage.GetItem<Data>(StorageKey) ?? _data;
        }
        catch (Exception)
        {
            storage.SetItem(StorageKey, _data);
        }

        ChangeTheme(_data.General.Theme);
    }

    internal static Language? GetLanguage(ILocalStorage storage)
    {
        Data? data = storage.GetItem<Data>(StorageKey);
        return data?.General.Language;
    }

    private void ChangeTheme(Theme theme) => jsRuntime.InvokeVoid("changeTheme", theme.ToString());

    [JSImport("globalThis.localStorage.clear")]
    private static partial void Clear();

    private sealed record Data
    {
        public General General { get; init; } = new();

        public FlagDifficulty Flag { get; init; }

        public CountryDifficulty Country { get; init; }
    }
}
