// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Geography;

namespace Atlas.Domain.Countries;

public sealed record Country
{
    public required Cca2 Cca2 { get; init; }

    public required IEnumerable<Capital> Capitals { get; init; }

    public required IEnumerable<Cca2> Borders { get; init; }

    public required Continent Continent { get; init; }

    public required Coordinate Coordinate { get; init; }

    public required Area Area { get; init; }

    public required int Population { get; init; }

    public required Resources Resources { get; init; }
}
