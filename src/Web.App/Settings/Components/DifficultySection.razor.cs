// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using System.Text;

namespace Web.App.Settings.Components;

public partial class DifficultySection
{
    [CascadingParameter, EditorRequired]
    public required AppState State { get; init; }

    private string CountryCss
    {
        get
        {
            if (State.Country == CountryDifficulty.None)
                return string.Empty;

            StringBuilder builder = new();

            if (HasCountryDifficulty(CountryDifficulty.Blur))
                builder.Append("filter: blur(3px);");

            List<string> transforms = [];

            if (HasCountryDifficulty(CountryDifficulty.Mirrored))
                transforms.Add("scaleX(-1)");

            if (HasCountryDifficulty(CountryDifficulty.Rotated))
                transforms.Add("rotate(45deg)");

            if (transforms.Count > 0)
                builder.Append($"transform: {string.Join(' ', transforms)};");

            return builder.ToString();
        }
    }

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
