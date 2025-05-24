// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using Web.App.Components.Modals;

namespace Web.App.Components;

public sealed partial class Header
{
    [CascadingParameter]
    public required SettingsModal SettingsModal { get; init; }

    [SupportedOSPlatform("browser")]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSHost.ImportAsync("header", "/scripts/header.js");

            ResponsiveExpand();
        }
    }

    [JSImport("toggleMenu", "header")]
    private static partial void ToggleMenu();

    [JSImport("responsiveExpand", "header")]
    private static partial void ResponsiveExpand();
}
