// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Microsoft.AspNetCore.Components;
using Web.App.Settings;
using Web.App.Storage;

namespace Web.App.Games.Countries;

public partial class DailyCountry(IDailyLocalStorage storage, IGetDailyCountry dailyHandler, IGuessCountry guessHandler)
{
    private const int MaxAttempts = 6;

    private int? _rotation;

    private readonly GameState _gameState = new(null, MaxAttempts);

    [CascadingParameter]
    public required AppSettings Settings { get; init; }

    private string DifficultyCss
    {
        get
        {
            if (HasRotation())
            {
                _rotation ??= Random.Shared.Next(0, 360);
                return string.Empty;
            }

            _rotation = null;
            return Settings.CountryDifficultyCss(Settings.Country.Random, _gameState.GameFinished ? MaxAttempts : _gameState.Guesses.Count);

            bool HasRotation() => Settings.Country.All is CountryDifficulty.Rotated || Settings.Country.Daily is CountryDifficulty.Rotated;
        }
    }

    private string Rotation => !_rotation.HasValue ? string.Empty : $"transform: rotate({_rotation}deg);";

    protected override async Task OnInitializedAsync()
    {
        CountryResponse? country = await dailyHandler.HandleAsync(CancellationToken.None);

        (GuessedCountryResponse[] guesses, bool gaveUp) = storage.Get(LocalStorageKeys.Country);

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
        storage.Set(LocalStorageKeys.Country, _gameState.Guesses);
    }

    private void GiveUp()
    {
        _gameState.GiveUp();
        storage.Set(LocalStorageKeys.Country, giveUp: true);
    }
}
