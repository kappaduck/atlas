// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Web.App.Dialogs;

public sealed partial class SettingsDialog : IDisposable
{
    private TabItem? _selectedTab;
    private CancellationTokenSource? _cts;

    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }

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

    public void Dispose() => ResetToken();

    public void ShowGeneralSection() => Show(TabItem.General);

    public void ShowChangelogSection() => Show(TabItem.Changelog);

    private void Show(TabItem tab)
    {
        _cts = new CancellationTokenSource();

        SelectTab(tab);
        Show();
    }

    private void SelectTab(TabItem tab)
    {
        _selectedTab = tab;
        ScrollContentToTop(".body");
    }

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
