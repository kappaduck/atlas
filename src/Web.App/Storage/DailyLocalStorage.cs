// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Web.App.Services;

namespace Web.App.Storage;

internal sealed class DailyLocalStorage(ILocalStorage storage, ITimeService timeService) : IDailyLocalStorage
{
    public (GuessedCountryResponse[] Guesses, bool GiveUp) Get(string key)
    {
        string guessedKey = GuessedKey(key);
        string giveUpKey = GiveUpKey(key);
        string todayKey = $"{key}:today";

        DateOnly today = timeService.Today;
        DateOnly lastPlayed = storage.GetItem<DateOnly>(todayKey);

        if (lastPlayed != today)
        {
            storage.RemoveItem(guessedKey);
            storage.RemoveItem(giveUpKey);
            storage.SetItem(todayKey, today);

            return ([], false);
        }

        GuessedCountryResponse[] guesses = storage.GetItem<GuessedCountryResponse[]>(guessedKey) ?? [];
        bool giveUp = storage.GetItem<bool>(giveUpKey);

        return (guesses, giveUp);
    }

    public void Set(string key, ICollection<GuessedCountryResponse> guesses)
        => storage.SetItem(GuessedKey(key), guesses);

    public void Set(string key, bool giveUp) => storage.SetItem(GiveUpKey(key), giveUp);

    private static string GuessedKey(string key) => $"{key}:guesses";

    private static string GiveUpKey(string key) => $"{key}:giveup";
}
