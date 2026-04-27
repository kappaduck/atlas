// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.AspNetCore.Components;

namespace Web.App.Settings.Components;

public sealed partial class CountriesSection(ICountryService service)
{
    private bool _isLoading;
    private CountryResponse[] _countries = [];
    private string? _search;

    [Parameter, EditorRequired]
    public required CancellationToken CancellationToken { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _countries = [.. (await service.GetAllAsync(CancellationToken)).OrderBy(c => c.Name)];

        _isLoading = false;
    }

    private Span<CountryResponse> GetFilteredCountries()
    {
        if (string.IsNullOrEmpty(_search))
            return _countries;

        return _countries.Where(c => c.Name.Contains(_search, StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}
