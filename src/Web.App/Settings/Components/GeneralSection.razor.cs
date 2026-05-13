// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Web.App.Settings.Components;

public sealed partial class GeneralSection
{
    [CascadingParameter]
    public required AppState State { get; init; }

    [Inject]
    private IStringLocalizer<Translations> Localizer { get; set; } = default!;

    private (Theme Theme, string Label)[] Themes
    {
        get
        {
            Theme[] themes = Enum.GetValues<Theme>();
            return [.. themes.Select(t => (t, Localizer[t.ToString()]))];
        }
    }

    private (Language Language, string Label)[] Languages
    {
        get
        {
            Language[] languages = Enum.GetValues<Language>();
            return [.. languages.Select(t => (t, Localizer[t.ToString()]))];
        }
    }

    private (DistanceUnit Unit, string Label)[] Units
    {
        get
        {
            DistanceUnit[] units = Enum.GetValues<DistanceUnit>();
            return [.. units.Select(t => (t, Localizer[t.ToString()]))];
        }
    }
}
