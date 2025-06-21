// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Settings;

public sealed record FlagDifficulty
{
    public Difficulty All { get; init; }

    public Difficulty Daily { get; init; }

    public Difficulty Random { get; init; }
}
