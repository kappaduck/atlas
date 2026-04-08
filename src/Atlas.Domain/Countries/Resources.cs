// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Countries;

public sealed record Resources
{
    public required Uri Map { get; init; }

    public required Uri Flag { get; init; }

    public Uri? CoatOfArms { get; init; }

    public required Uri Country { get; init; }
}
