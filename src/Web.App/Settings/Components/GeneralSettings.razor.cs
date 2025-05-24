// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Settings.Components;

public sealed partial class GeneralSettings
{
    [CascadingParameter]
    public required AppSettings AppSettings { get; init; }

    private static (Theme Theme, string Name)[] GetThemes()
        => [.. Enum.GetValues<Theme>().Select(t => (t, t.ToString()))];

    private static (Language Language, string Name)[] GetLanguages()
        => [.. Enum.GetValues<Language>().Select(l => (l, l.ToString()))];
}
