// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.AspNetCore.Components;
using Web.App.Games.Components;
using Web.App.Options;
using Web.App.Settings;

namespace Web.App.Games.Countries;

public sealed partial class Random(ICountryService service, DevOptions options) : IDisposable
{
    private const int MaxAttempts = 6;

    private readonly CancellationTokenSource _cts = new();
    private readonly GameState _gameState = new(MaxAttempts);

    private LookupInput _input = default!;
    private int? _rotation;

    [CascadingParameter]
    public required AppState State { get; init; }

    [Parameter]
    public string? Cca2 { get; init; }

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
        _rotation = System.Random.Shared.Next(0, 360);

        CountryResponse? country = await GetAsync(_cts.Token) ?? throw new InvalidOperationException();
        _gameState.Start(country);

        Task<CountryResponse?> GetAsync(CancellationToken cancellationToken)
        {
            if (options.Debug && !string.IsNullOrEmpty(Cca2))
                return service.GetAsync(Cca2, cancellationToken);

            return service.RandomizeAsync(cancellationToken);
        }
    }

    private async Task GuessAsync(string cca2)
    {
        GuessedCountryResponse? guessedCountry = await service.GuessAsync(cca2, _gameState.Country!.Cca2, _cts.Token) ?? throw new InvalidOperationException();
        _gameState.Guesses.Add(guessedCountry);
    }

    private async Task PlayAgainAsync()
    {
        _input.Reset();
        _rotation = System.Random.Shared.Next(0, 360);

        CountryResponse? country = await service.RandomizeAsync(_cts.Token) ?? throw new InvalidOperationException();
        _gameState.Reset(country);
    }
}
