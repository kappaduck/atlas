// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Web.App.Storage;

internal class DailyLocalStorage(string key, ILocalStorage storage) : IDailyLocalStorage
{
    internal const string Flag = "flag";
    internal const string Country = "country";

    private readonly string _key = $"daily:{key}";
    private Data _daily = new();

    public void Abandon() => storage.SetItem(_key, _daily with { Abandon = true });

    public void Add(GuessedCountryResponse guess)
    {
        _daily = _daily with { Guesses = [.. _daily.Guesses, guess] };
        storage.SetItem(_key, _daily);
    }

    public (IEnumerable<GuessedCountryResponse> Guesses, bool Abandon) Get()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        _daily = storage.GetItem<Data>(_key) ?? _daily;

        if (today != _daily.Today)
        {
            _daily = _daily with
            {
                Today = today,
                Abandon = false,
                Guesses = []
            };

            storage.SetItem(_key, _daily);
        }

        return (_daily.Guesses, _daily.Abandon);
    }

    private sealed record Data
    {
        public DateOnly Today { get; init; }

        public bool Abandon { get; init; }

        public GuessedCountryResponse[] Guesses { get; init; } = [];
    }
}
