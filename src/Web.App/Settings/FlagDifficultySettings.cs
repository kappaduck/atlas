// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Settings;

public sealed record FlagDifficultySettings
{
    public FlagDifficulty All { get; init; }

    public FlagDifficulty Daily { get; init; }

    public FlagDifficulty Random { get; init; }
}
