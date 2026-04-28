// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Application.Countries.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using Web.App.Extensions;

namespace Web.App.Games.Components;

public sealed partial class LookupInput(ICountryLookupService service, IJSInProcessRuntime jsRuntime) : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private string _input = string.Empty;
    private CountryLookupResponse[] _filteredCountries = [];
    private CountryLookupResponse[] _countries = [];
    private int _selectedIndex = -1;

    private IJSInProcessObjectReference? _module;
    private DotNetObjectReference<LookupInput>? _reference;

    [Parameter, EditorRequired]
    public EventCallback<string> OnLookup { get; init; }

    [CascadingParameter]
    public required GameState GameState { get; init; }

    [JSInvokable]
    public void Clear()
    {
        _filteredCountries = [];
        _selectedIndex = -1;

        StateHasChanged();
    }

    public void Dispose()
    {
        _module?.InvokeVoid("dispose");

        _module?.Dispose();
        _module = null;

        _reference?.Dispose();
        _reference = null;

        _cts.Cancel();
        _cts.Dispose();
    }

    public void Reset()
    {
        _input = string.Empty;
        _filteredCountries = [];
        _selectedIndex = -1;
    }

    protected override async Task OnInitializedAsync() => _countries = [.. (await service.LookupAsync(_cts.Token)).OrderBy(c => c.Name)];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _module = await jsRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "/scripts/lookup.js");

        _reference = DotNetObjectReference.Create(this);
        _module?.InvokeVoid("init", _reference);
    }

    private string IsActive(int index) => _selectedIndex == index ? "active" : string.Empty;

    private Task SelectCountryAsync(string cca2)
    {
        Reset();
        return OnLookup.InvokeAsync(cca2);
    }

    private void Lookup(ChangeEventArgs e)
    {
        _selectedIndex = -1;

        _input = e.Value?.ToString() ?? string.Empty;
        _filteredCountries = LookupCountries();
    }

    private Task HandleKeyboardAsync(KeyboardEventArgs e)
    {
        if (e.Key == Keyboard.Escape)
        {
            _filteredCountries = [];
            return Task.CompletedTask;
        }

        if (e.Key == Keyboard.Enter && TrySelectCountry(out string? cca2))
            return SelectCountryAsync(cca2);

        HandleNavigation(e.Key);
        return Task.CompletedTask;
    }

    private void HandleNavigation(string key)
    {
        if (key == Keyboard.ArrowDown)
            _selectedIndex = (_selectedIndex + 1) % _filteredCountries.Length;

        if (key == Keyboard.ArrowUp)
        {
            int index = _selectedIndex == -1 ? 0 : _selectedIndex;
            _selectedIndex = (index - 1 + _filteredCountries.Length) % _filteredCountries.Length;
        }

        _module?.InvokeVoid("scrollToCountry", $"country-{_selectedIndex}");
    }

    private void Focus()
    {
        _module?.InvokeVoid("scrollToLookup");
        _filteredCountries = LookupCountries();

        StateHasChanged();
    }

    private bool TrySelectCountry([NotNullWhen(true)] out string? cca2)
    {
        cca2 = null;

        if (_filteredCountries.Length == 1)
        {
            cca2 = _filteredCountries[0].Cca2;
            return true;
        }

        if (_selectedIndex != -1)
        {
            cca2 = _filteredCountries[_selectedIndex].Cca2;
            return true;
        }

        if (_filteredCountries.Length > 1)
        {
            string input = string.RemoveDiacritics(_input.Trim());
            CountryLookupResponse? country = Array.Find(_filteredCountries, c => Compare(c.Name, input));

            cca2 = country?.Cca2;
            return cca2 is not null;
        }

        return false;

        static bool Compare(string name, ReadOnlySpan<char> input)
        {
            ReadOnlySpan<char> normalized = string.RemoveDiacritics(name);

            if (normalized.Equals(input, StringComparison.OrdinalIgnoreCase))
                return true;

            Span<char> initials = stackalloc char[7];
            int length = string.CreateInitials(normalized, initials);

            return initials[..length].Equals(input, StringComparison.OrdinalIgnoreCase);
        }
    }

    private CountryLookupResponse[] LookupCountries()
    {
        string input = string.RemoveDiacritics(_input.Trim());
        CountryLookupResponse[] availableCountries = [.. _countries.ExceptBy(GameState.Guesses.Select(g => g.Cca2), c => c.Cca2)];

        return Array.FindAll(availableCountries, c => string.Lookup(c.Name, input));
    }

    private static class Keyboard
    {
        internal const string ArrowDown = "ArrowDown";
        internal const string ArrowUp = "ArrowUp";
        internal const string Escape = "Escape";
        internal const string Enter = "Enter";
    }
}
