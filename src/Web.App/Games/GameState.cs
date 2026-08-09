// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Web.App.Games;

public sealed class GameState(int maxAttempts)
{
    public bool Abandon { get; private set; }

    public CountryResponse? Country { get; private set; }

    public int MaxAttempts { get; } = maxAttempts;

    public ICollection<GuessedCountryResponse> Guesses { get; private set; } = [with(maxAttempts)];

    public bool GameFinished => Abandon || Guesses.Count == MaxAttempts || Guesses.Any(g => g.Success);

    public bool Found => Guesses.Any(g => g.Success);

    public void GiveUp() => Abandon = true;

    public void Reset(CountryResponse country)
    {
        Guesses.Clear();
        Country = country;
    }

    public void Start(CountryResponse country) => Country = country;

    public void Start(CountryResponse country, IEnumerable<GuessedCountryResponse> guesses, bool abandon = false)
    {
        Country = country;
        Guesses = [.. guesses];
        Abandon = abandon;
    }
}
