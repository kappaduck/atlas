// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Queries;
using Mediator;

namespace Web.App.Games.Flags;

public sealed partial class RandomizedFlag(IMediator mediator)
{
    private CountryResponse? _country;

    protected override async Task OnInitializedAsync()
        => _country = await mediator.Send(new RandomizeCountry.Query());
}
