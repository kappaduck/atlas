// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.AspNetCore.Components;
using Web.App.Games.Components;
using Web.App.Settings;
using Web.App.Storage;

namespace Web.App.Games.Flags;

public sealed partial class Random(ICountryService service, ILocalStorage storage) : IDisposable
{
    private const int MaxAttempts = 6;

    private readonly CancellationTokenSource _cts = new();
    private readonly GameState _gameState = new(MaxAttempts);

    private Score _score = new("random:flag:streak");
    private CountryInput _input = default!;

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
        GuessedCountryResponse? guessedCountry = await service.GuessAsync(cca2, _gameState.Country!.Cca2, _cts.Token);

        _gameState.Guesses.Add(guessedCountry!);

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

        _score.Reset();
        storage.SetItem(_score.Key, _score);
    }

    private async Task FetchGameAsync()
    {
        try
        {
            _isLoading = true;
            CountryResponse? country = await service.RandomizeAsync(_cts.Token);

            if (country is null)
                return;

            _score = storage.GetItem<Score>(_score.Key) ?? _score;
            _gameState.Start(country);
        }
        catch (Exception)
        {
            _hasError = true;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task PlayAgainAsync()
    {
        _input.Reset();

        try
        {
            _isLoading = true;
            CountryResponse? country = await service.RandomizeAsync(_cts.Token);

            if (country is null)
                return;

            _gameState.Reset(country);
        }
        catch (Exception)
        {
            _hasError = true;
        }
        finally
        {
            _isLoading = false;
        }
    }
}
