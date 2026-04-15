// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Web.App.Games;

public sealed record DailyGame
{
    public DateOnly Today { get; init; }

    public GuessedCountryResponse[] Guesses { get; init; } = [];

    public bool Abandon { get; init; }
}
