// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Web.App.Settings;

public sealed partial class SettingsDialog(IJSInProcessRuntime jsRuntime) : IDisposable
{
    private ElementReference _dialog;
    private TabItem? _selectedTab;
    private CancellationTokenSource? _cts;
    private DotNetObjectReference<SettingsDialog>? _reference;

    [Inject]
    private IStringLocalizer<Translations> Localizer { get; set; } = default!;

    private (TabItem Tab, string Label)[] Tabs
    {
        get
        {
            TabItem[] tabs = Enum.GetValues<TabItem>();
            return [.. tabs.Select(t => (t, Localizer[t.ToString()]))];
        }
    }

    public void Dispose()
    {
        ResetToken();
        _reference?.Dispose();
    }

    [JSInvokable]
    public void ShowGeneralSection()
    {
        Show(TabItem.General);
        StateHasChanged();
    }

    [JSInvokable]
    public void ShowChangelogSection()
    {
        Show(TabItem.Changelog);
        StateHasChanged();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;

        _reference = DotNetObjectReference.Create(this);
        jsRuntime.InvokeVoid("initSettings", _dialog, _reference);
    }

    private void Show(TabItem tab)
    {
        _cts = new CancellationTokenSource();
        SelectTab(tab);
    }

    private void SelectTab(TabItem tab) => _selectedTab = tab;

    private string IsActive(TabItem tab) => _selectedTab == tab ? "active" : string.Empty;

    private void ResetToken()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    private enum TabItem
    {
        General = 0,
        Difficulty = 1,
        Changelog = 2,
        Countries = 3
    }
}
