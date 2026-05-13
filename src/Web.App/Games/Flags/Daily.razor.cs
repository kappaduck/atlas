// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.AspNetCore.Components;
using Web.App.Settings;
using Web.App.Storage;

namespace Web.App.Games.Flags;

public sealed partial class Daily(ICountryService service, NavigationManager navigation, [FromKeyedServices(DailyFlagStorage.Key)] IDailyLocalStorage storage) : IDisposable
{
    private const int MaxAttempts = 6;

    private readonly CancellationTokenSource _cts = new();
    private readonly GameState _gameState = new(MaxAttempts);

    [CascadingParameter]
    public required AppState State { get; init; }

    private string FoundCss
    {
        get
        {
            if (!_gameState.GameFinished)
                return string.Empty;

            return _gameState.Found ? "success" : "wrong";
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    protected override async Task OnInitializedAsync()
    {
        CountryResponse? country = await service.GetDailyFlagAsync(_cts.Token) ?? throw new InvalidOperationException();
        IEnumerable<GuessedCountryResponse> guesses = storage.Get();

        _gameState.Start(country, guesses);
    }

    private async Task GuessAsync(string cca2)
    {
        GuessedCountryResponse? guessedCountry = await service.GuessAsync(cca2, _gameState.Country!.Cca2, _cts.Token) ?? throw new InvalidOperationException();

        _gameState.Guesses.Add(guessedCountry);
        storage.Set(_gameState.Guesses);
    }

    private void NavigateToRandom() => navigation.NavigateTo("/flags/random");
}
