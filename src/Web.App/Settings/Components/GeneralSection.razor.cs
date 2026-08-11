// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Runtime.InteropServices.JavaScript;

namespace Web.App.Settings.Components;

public sealed partial class GeneralSection(IStringLocalizer<AppLocalizer> localizer)
{
    [CascadingParameter]
    public required AppState State { get; init; }

    private (Language Language, string Label)[] Languages
    {
        get
        {
            Language[] languages = Enum.GetValues<Language>();
            return [.. languages.Select(t => (t, localizer[t.ToString()]))];
        }
    }

    private (DistanceUnit Unit, string Label)[] Units
    {
        get
        {
            DistanceUnit[] units = Enum.GetValues<DistanceUnit>();
            return [.. units.Select(t => (t, localizer[t.ToString()]))];
        }
    }

    private void ClearStorage()
    {
        if (Confirm(Localizer.ConfirmClearData))
            State.Reset();
    }

    [JSImport("globalThis.confirm")]
    private static partial bool Confirm(string message);
}
