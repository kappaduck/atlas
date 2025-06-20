// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;

namespace Web.App.Games;

public sealed class GameState(CountryResponse? country, int maxAttempts)
{
    public CountryResponse? Country { get; private set; } = country;

    public int MaxAttempts { get; } = maxAttempts;

    public ICollection<GuessedCountryResponse> Guesses { get; } = new List<GuessedCountryResponse>(maxAttempts);

    public bool GameFinished
    {
        get => field || Guesses.Count == MaxAttempts || Guesses.Any(g => g.Success);
        private set;
    }

    public void Reset(CountryResponse? country)
    {
        Guesses.Clear();
        GameFinished = false;
        Country = country;
    }

    public void GiveUp() => GameFinished = true;
}
