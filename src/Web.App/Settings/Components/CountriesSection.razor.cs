// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.AspNetCore.Components;
using Web.App.Extensions;

namespace Web.App.Settings.Components;

public sealed partial class CountriesSection(ICountryService service)
{
    private bool _isLoading;
    private bool _hasError;
    private string? _search;
    private CountryListItem[] _countries = [];
    private CountryListItem[] _filteredCountries = [];

    [Parameter, EditorRequired]
    public required CancellationToken CancellationToken { get; init; }

    protected override Task OnInitializedAsync() => FetchCountriesAsync();

    private async Task FetchCountriesAsync()
    {
        _hasError = false;
        _isLoading = true;

        try
        {
            _countries = [.. (await service.GetAllAsync(CancellationToken)).OrderBy(c => c.Name)];
            _filteredCountries = _countries;
        }
        catch (HttpRequestException)
        {
            _hasError = true;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void Search(string? search)
    {
        _search = search;

        if (string.IsNullOrEmpty(_search))
        {
            _filteredCountries = _countries;
            return;
        }

        _filteredCountries = [.. _countries.Where(c => string.Lookup(c.Name, _search))];
    }
}
