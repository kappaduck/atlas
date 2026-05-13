// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Web.App.Games;

namespace Web.App.Storage;

internal abstract class DailyLocalStorage(string key, ILocalStorage storage) : IDailyLocalStorage
{
    private readonly string _key = $"daily:{key}";
    private DailyGame _daily = default!;

    public IEnumerable<GuessedCountryResponse> Get()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        _daily = storage.GetItem<DailyGame>(_key) ?? new DailyGame();

        if (today != _daily.Today)
        {
            _daily = _daily with
            {
                Today = today,
                Guesses = []
            };

            storage.SetItem(_key, _daily);
        }

        return _daily.Guesses;
    }

    public void Set(IEnumerable<GuessedCountryResponse> guesses)
    {
        storage.SetItem(_key, _daily with
        {
            Guesses = [.. guesses]
        });
    }
}
