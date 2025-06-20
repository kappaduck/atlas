// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Mediator;
using Web.App.Components.Modals;
using Web.App.Services;
using Web.App.Storage;

namespace Web.App.Games.Flags;

public sealed partial class DailyFlag(ILocalStorage storage, IMediator mediator, ITimeService timeService)
{
    private const int MaxAttempts = 6;

    private ZoomModal _zoomModal = default!;

    private GameState _gameState = default!;

    protected override async Task OnInitializedAsync()
    {
        CountryResponse? country = await mediator.Send(new GetDailyCountry.Query());

        DateOnly today = timeService.Today;
        DateOnly lastPlayed = storage.GetItem<DateOnly>(LocalStorageKeys.Today);

        if (lastPlayed != today)
        {
            storage.RemoveItem(LocalStorageKeys.Guesses);
            storage.RemoveItem(LocalStorageKeys.GiveUp);
            storage.SetItem(LocalStorageKeys.Today, today);
        }

        _gameState = new GameState(country, MaxAttempts);

        if (storage.GetItem<bool>(LocalStorageKeys.GiveUp))
            _gameState.GiveUp();

        foreach (GuessedCountryResponse guess in storage.GetItem<GuessedCountryResponse[]>(LocalStorageKeys.Guesses) ?? [])
            _gameState.Guesses.Add(guess);
    }

    private async Task GuessAsync(string cca2)
    {
        GuessedCountryResponse guessedCountry = await mediator.Send(new GuessCountry.Command(cca2, _gameState.Country!.Cca2));

        _gameState.Guesses.Add(guessedCountry);
        storage.SetItem(LocalStorageKeys.Guesses, _gameState.Guesses);
    }

    private void GiveUp()
    {
        _gameState.GiveUp();
        storage.SetItem(LocalStorageKeys.GiveUp, value: true);
    }
}
