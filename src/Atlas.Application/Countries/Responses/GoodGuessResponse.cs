// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Countries.Responses;

public sealed record GoodGuessResponse
{
    public required string Cca2 { get; init; }

    public required string Name { get; init; }

    public required string Continent { get; init; }

    public required Uri Flag { get; init; }

    public required Uri Map { get; init; }
}
