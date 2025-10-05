// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Microsoft.AspNetCore.Components;
using Web.App.Components.Modals;
using Web.App.Settings;
using Web.App.Storage;

namespace Web.App.Games.Flags;

public sealed partial class DailyFlag(IDailyLocalStorage storage, IGetDailyFlag dailyHandler, IGuessCountry guessHandler)
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

        (GuessedCountryResponse[] guesses, bool gaveUp) = storage.Get(LocalStorageKeys.Flag);

        _gameState.Reset(country);

        if (gaveUp)
            _gameState.GiveUp();

        foreach (GuessedCountryResponse guess in guesses)
            _gameState.Guesses.Add(guess);
    }

    private async Task GuessAsync(string cca2)
    {
        GuessedCountryResponse guessedCountry = await guessHandler.HandleAsync(cca2, _gameState.Country!.Cca2, CancellationToken.None);

        _gameState.Guesses.Add(guessedCountry);
        storage.Set(LocalStorageKeys.Flag, _gameState.Guesses);
    }

    private void GiveUp()
    {
        _gameState.GiveUp();
        storage.Set(LocalStorageKeys.Flag, giveUp: true);
    }
}
