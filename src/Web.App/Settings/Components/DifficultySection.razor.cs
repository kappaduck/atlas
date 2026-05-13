// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Web.App.Settings.Components;

public sealed partial class DifficultySection
{
    private const string Flag = "https://flagcdn.com/ca.svg";
    private const string Country = "https://cdn.jsdelivr.net/gh/kappaduck/atlas/countries/ca/country.svg";

    [CascadingParameter]
    public required AppState State { get; init; }

    private bool HasCountryDifficulty(CountryDifficulty difficulty)
        => (State.Country & difficulty) == difficulty;

    private void SetCountryDifficulty(CountryDifficulty difficulty)
    {
        if (HasCountryDifficulty(difficulty))
        {
            State.Country &= ~difficulty;
            return;
        }

        State.Country |= difficulty;
    }
}
