// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Settings;

public sealed record General
{
    public Theme Theme { get; init; }

    public Language Language { get; init; }

    public DistanceUnit Unit { get; init; }

    public bool DistanceHint { get; init; } = true;

    public bool ArrowHint { get; init; } = true;

    public bool ContinentHint { get; init; } = true;

    public bool ProximityBarHint { get; init; } = true;

    public bool FlagHint { get; init; } = true;

    public bool GuessFlagHint { get; init; } = true;
}
