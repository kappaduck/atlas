// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Microsoft.AspNetCore.Components;
using Web.App.Components.Modals;
using Web.App.Games.Components;
using Web.App.Options;
using Web.App.Settings;

namespace Web.App.Games.Flags;

public sealed partial class RandomizedFlag(IRandomizeCountry randomizeHandler, IGetCountry getHandler, IGuessCountry guessHandler, FeatureOptions options)
{
    private const int MaxAttempts = 6;

    private CountryLookupInput _input = default!;

    private readonly GameState _gameState = new(null, MaxAttempts);

    [CascadingParameter]
    public required AppSettings Settings { get; init; }

    [CascadingParameter]
    public required ZoomModal ZoomModal { get; init; }

    [Parameter, SupplyParameterFromQuery(Name = "cca2")]
    public string? Cca2 { get; set; }

    private string DifficultyCss => Settings.DifficultyCss(Settings.Flag.Random, _gameState.Guesses.Count);

    protected override async Task OnInitializedAsync()
    {
        CountryResponse? country = await GetCountryAsync();
        _gameState.Reset(country);

        ValueTask<CountryResponse?> GetCountryAsync()
        {
            if (options.DevMode && !string.IsNullOrEmpty(Cca2))
                return getHandler.HandleAsync(Cca2, CancellationToken.None);

            return randomizeHandler.HandleAsync(CancellationToken.None);
        }
    }

    private async Task GuessAsync(string cca2)
    {
        GuessedCountryResponse guessedCountry = await guessHandler.HandleAsync(cca2, _gameState.Country!.Cca2, CancellationToken.None);

        _gameState.Guesses.Add(guessedCountry);
    }

    private async Task PlayAgainAsync()
    {
        _input.Reset();

        CountryResponse? country = await randomizeHandler.HandleAsync(CancellationToken.None);
        _gameState.Reset(country);
    }

    private void GiveUp() => _gameState.GiveUp();
}
