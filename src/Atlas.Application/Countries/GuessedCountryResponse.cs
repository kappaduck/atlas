// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Countries;

public sealed record GuessedCountryResponse
{
    public required string Cca2 { get; init; }

    public required string Name { get; init; }

    public required bool IsSameContinent { get; init; }

    public required int Kilometers { get; init; }

    public required double Direction { get; init; }

    public required bool Success { get; init; }

    public required ImageResponse Flag { get; init; }
}
