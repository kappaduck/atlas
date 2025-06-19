// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;
using Atlas.Application.Countries.Queries;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text;

namespace Web.App.Games.Components;

public sealed partial class CountryLookupInput(IMediator mediator, IJSInProcessRuntime jsRuntime) : IDisposable
{
    private string _input = string.Empty;
    private CountryLookupResponse[] _filteredCountries = [];
    private CountryLookupResponse[] _countries = [];

    private IJSInProcessObjectReference? _module;

    private DotNetObjectReference<CountryLookupInput>? _reference;

    [Parameter, EditorRequired]
    public EventCallback<string> Guess { get; init; }

    [Parameter]
    public IEnumerable<string> GuessedCountries { get; init; } = [];

    public void Reset()
    {
        _input = string.Empty;
        _filteredCountries = [];
    }

    [JSInvokable]
    public void Clear()
    {
        _filteredCountries = [];
        StateHasChanged();
    }

    public void Dispose()
    {
        _module?.InvokeVoid("dispose");

        _module?.Dispose();
        _module = null;

        _reference?.Dispose();
        _reference = null;
    }

    protected override async Task OnInitializedAsync() => _countries = await mediator.Send(new LookupCountries.Query());

    [SupportedOSPlatform("browser")]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _module = await jsRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "/scripts/lookup.js");

        _reference = DotNetObjectReference.Create(this);
        _module?.InvokeVoid("init", _reference);
    }

    private void Lookup(ChangeEventArgs e)
    {
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

        return Task.CompletedTask;
    }

    private void Focus()
    {
        _module?.InvokeVoid("scrollToLookup");
        _filteredCountries = LookupCountries();
    }

    private Task SelectCountryAsync(string cca2)
    {
        _input = string.Empty;
        _filteredCountries = [];

        return Guess.InvokeAsync(cca2);
    }

    private bool TrySelectCountry([NotNullWhen(true)] out string? cca2)
    {
        cca2 = null;

        if (_filteredCountries.Length == 1)
        {
            cca2 = _filteredCountries[0].Cca2;
            return true;
        }

        if (_filteredCountries.Length > 1)
        {
            string input = RemoveDiacritics(_input.Trim());

            CountryLookupResponse? country = Array.Find(_filteredCountries, c => Lookup(c.Name, input));

            cca2 = country?.Cca2;
            return cca2 is not null;
        }

        return false;

        static bool Lookup(string name, ReadOnlySpan<char> input)
        {
            ReadOnlySpan<char> normalized = RemoveDiacritics(name);

            if (normalized.Equals(input, StringComparison.OrdinalIgnoreCase))
                return true;

            Span<char> initials = stackalloc char[7];
            int length = CreateInitials(normalized, initials);

            return initials[..length].Equals(input, StringComparison.OrdinalIgnoreCase);
        }
    }

    private CountryLookupResponse[] LookupCountries()
    {
        string input = RemoveDiacritics(_input.Trim());
        CountryLookupResponse[] availableCountries = [.. _countries.ExceptBy(GuessedCountries, c => c.Cca2)];

        return Array.FindAll(availableCountries, c => Lookup(c.Name, input));

        static bool Lookup(string name, ReadOnlySpan<char> input)
        {
            ReadOnlySpan<char> normalized = RemoveDiacritics(name);

            if (normalized.Contains(input, StringComparison.OrdinalIgnoreCase))
                return true;

            Span<char> initials = stackalloc char[7];
            int length = CreateInitials(normalized, initials);

            return initials[..length].Contains(input, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static string RemoveDiacritics(string value)
    {
        ReadOnlySpan<char> normalized = value.Normalize(NormalizationForm.FormD);

        return string.Create(value.Length, normalized, (buffer, content) =>
        {
            int i = 0;
            foreach (char c in content)
            {
                if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    buffer[i++] = c;
            }
        });
    }

    private static int CreateInitials(ReadOnlySpan<char> value, Span<char> initials)
    {
        int i = 0;
        foreach (Range range in value.Split(' '))
        {
            ReadOnlySpan<char> word = value[range];
            initials[i++] = word[0];
        }

        return i;
    }

    private static class Keyboard
    {
        internal const string Escape = "Escape";
        internal const string Enter = "Enter";
    }
}
