// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Commands;
using Atlas.Application.Countries.Queries;
using Mediator;
using Microsoft.AspNetCore.Components;
using Web.App.Components.Modals;
using Web.App.Games.Components;
using Web.App.Settings;

namespace Web.App.Games.Flags;

public sealed partial class RandomizedFlag(IMediator mediator)
{
    private const int MaxAttempts = 6;

    private CountryLookupInput _input = default!;

    private readonly GameState _gameState = new(null, MaxAttempts);

    [CascadingParameter]
    public required AppSettings Settings { get; init; }

    [CascadingParameter]
    public required ZoomModal ZoomModal { get; init; }

    private string DifficultyCss => Settings.DifficultyCss(Settings.Flag.Random, _gameState.Guesses.Count);

    protected override async Task OnInitializedAsync()
    {
        CountryResponse? country = await mediator.Send(new RandomizeCountry.Query());

        _gameState.Reset(country);
    }

    private async Task GuessAsync(string cca2)
    {
        GuessedCountryResponse guessedCountry = await mediator.Send(new GuessCountry.Command(cca2, _gameState.Country!.Cca2));

        _gameState.Guesses.Add(guessedCountry);
    }

    private async Task PlayAgainAsync()
    {
        _input.Reset();

        CountryResponse? country = await mediator.Send(new RandomizeCountry.Query());
        _gameState.Reset(country);
    }

    private void GiveUp() => _gameState.GiveUp();
}
