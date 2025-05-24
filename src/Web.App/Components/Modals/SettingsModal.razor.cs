// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Components.Modals;

public sealed partial class SettingsModal
{
    private TabItem? _selectedTab;
    private ElementReference _dialog;

    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }

    public void ShowGeneral() => Show(TabItem.General);

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
            return;

        CloseModalOnClickOutside(_dialog);
    }

    private static (TabItem Item, string Name)[] GetTabs()
        => [.. Enum.GetValues<TabItem>().Select(t => (t, t.ToString()))];

    private void Show(TabItem tab)
    {
        SelectTab(tab);

        Show(_dialog);
        ScrollContentToTop(_dialog);
    }

    private void SelectTab(TabItem tab) => _selectedTab = tab;

    private string IsActive(TabItem tab) => _selectedTab == tab ? "active" : string.Empty;

    private string GetTabCss() => _selectedTab switch
    {
        TabItem.General => "general",
        _ => string.Empty
    };

    private enum TabItem
    {
        General = 0
    }
}
