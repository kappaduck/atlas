// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace Web.App.Components;

public sealed partial class Header
{
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
