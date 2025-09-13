// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Settings;

public sealed record CountryDifficultySettings
{
    public CountryDifficulty All { get; init; }

    public CountryDifficulty Daily { get; init; }

    public CountryDifficulty Random { get; init; }
}
