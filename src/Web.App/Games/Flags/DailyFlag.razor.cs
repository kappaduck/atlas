// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Microsoft.AspNetCore.Components;
using Web.App.Components.Modals;
using Web.App.Services;
using Web.App.Settings;
using Web.App.Storage;

namespace Web.App.Games.Flags;

public sealed partial class DailyFlag(ILocalStorage storage, IGetDailyCountry dailyHandler, IGuessCountry guessHandler, ITimeService timeService)
{
    private const int MaxAttempts = 6;

    private readonly GameState _gameState = new(null, MaxAttempts);

    [CascadingParameter]
    public required AppSettings Settings { get; init; }

    [CascadingParameter]
    public required ZoomModal ZoomModal { get; init; }

    private string DifficultyCss => Settings.FlagDifficultyCss(Settings.Flag.Daily, _gameState.Guesses.Count);

    protected override async Task OnInitializedAsync()
    {
        CountryResponse? country = await dailyHandler.HandleAsync(CancellationToken.None);

        DateOnly today = timeService.Today;
        DateOnly lastPlayed = storage.GetItem<DateOnly>(LocalStorageKeys.Today);

        if (lastPlayed != today)
        {
            storage.RemoveItem(LocalStorageKeys.Guesses);
            storage.RemoveItem(LocalStorageKeys.GiveUp);
            storage.SetItem(LocalStorageKeys.Today, today);
        }

        _gameState.Reset(country);

        if (storage.GetItem<bool>(LocalStorageKeys.GiveUp))
            _gameState.GiveUp();

        foreach (GuessedCountryResponse guess in storage.GetItem<GuessedCountryResponse[]>(LocalStorageKeys.Guesses) ?? [])
            _gameState.Guesses.Add(guess);
    }

    private async Task GuessAsync(string cca2)
    {
        GuessedCountryResponse guessedCountry = await guessHandler.HandleAsync(cca2, _gameState.Country!.Cca2, CancellationToken.None);

        _gameState.Guesses.Add(guessedCountry);
        storage.SetItem(LocalStorageKeys.Guesses, _gameState.Guesses);
    }

    private void GiveUp()
    {
        _gameState.GiveUp();
        storage.SetItem(LocalStorageKeys.GiveUp, value: true);
    }
}
