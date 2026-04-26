// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Web.App.Games;

namespace Web.App.Storage;

internal abstract class DailyLocalStorage(string key, ILocalStorage storage) : IDailyLocalStorage
{
    private readonly string _key = $"daily:{key}";

    public DailyGame Get()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DailyGame game = storage.GetItem<DailyGame>(_key) ?? new DailyGame();

        if (today != game.Today)
        {
            game = game with
            {
                Today = today,
                Abandon = false,
                Guesses = []
            };

            storage.SetItem(_key, game);
        }

        return game;
    }

    public void Set(DailyGame game) => storage.SetItem(_key, game);
}
