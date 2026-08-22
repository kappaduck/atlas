// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.AspNetCore.Components;
using Web.App.Settings;
using Web.App.Storage;

namespace Web.App.Games.Flags;

public sealed partial class Daily(ICountryService service, [FromKeyedServices(DailyLocalStorage.Flag)] IDailyLocalStorage daily, ILocalStorage storage) : IDisposable
{
    private const int MaxAttempts = 6;

    private readonly CancellationTokenSource _cts = new();
    private readonly GameState _gameState = new(MaxAttempts);

    private Score _score = new("daily:flag:streak");

    private bool _hasError;
    private bool _isLoading;
    private bool _preview;

    [CascadingParameter]
    public required AppState State { get; init; }

    private string DifficultyCss
    {
        get
        {
            if (State.Flag == FlagDifficulty.None || (_gameState.GameFinished && !_preview))
                return string.Empty;

            return State.Flag switch
            {
                FlagDifficulty.Blur => "blur",
                FlagDifficulty.Grayscale => "grayscale",
                FlagDifficulty.Invert => "invert",
                FlagDifficulty.Shift => "shift",
                _ => string.Empty
            };
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    protected override Task OnInitializedAsync() => FetchGameAsync();

    private async Task GuessAsync(string cca2)
    {
        GuessedResponse guessedCountry = await service.GuessAsync(cca2, _gameState.Country!.Cca2, _cts.Token);

        _gameState.Guesses.Add(guessedCountry);
        daily.Add(guessedCountry);

        if (_gameState.GameFinished)
        {
            if (_gameState.Found)
                _score.Increment();
            else
                _score.Reset();

            storage.SetItem(_score.Key, _score);
        }
    }

    private void GiveUp()
    {
        _gameState.GiveUp();
        daily.Abandon();

        _score.Reset();
        storage.SetItem(_score.Key, _score);
    }

    private async Task FetchGameAsync()
    {
        try
        {
            _isLoading = true;
            CountryResponse country = await service.GetDailyFlagAsync(_cts.Token);

            _score = storage.GetItem<Score>(_score.Key) ?? _score;

            (IEnumerable<GuessedResponse> guesses, bool abandon) = daily.Get();
            _gameState.Start(country, guesses, abandon);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            _hasError = true;
        }
        finally
        {
            _isLoading = false;
        }
    }
}
