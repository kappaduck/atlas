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

public sealed partial class CountryInput(ICountryLookupService service, IJSInProcessRuntime jsRuntime) : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private string _search = string.Empty;
    private CountryLookupResponse[] _filteredCountries = [];
    private CountryLookupResponse[] _countries = [];
    private int _selectedIndex = -1;

    private IJSInProcessObjectReference? _module;
    private DotNetObjectReference<CountryInput>? _reference;

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
        _search = string.Empty;
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

    private bool IsActive(int index) => _selectedIndex == index;

    private Task SelectCountryAsync(string cca2)
    {
        Reset();
        return OnLookup.InvokeAsync(cca2);
    }

    private void Lookup(ChangeEventArgs e)
    {
        _selectedIndex = -1;

        _search = e.Value?.ToString() ?? string.Empty;
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

    private void Focus() => _filteredCountries = LookupCountries();

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
            string input = string.RemoveDiacritics(_search.Trim());
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
        string input = string.RemoveDiacritics(_search.Trim());
        CountryLookupResponse[] availableCountries = [.. _countries.ExceptBy(GameState.Guesses.Select(g => g.Cca2), c => c.Cca2)];

        return Array.FindAll(availableCountries, c => string.Lookup(c.Name, input));
    }

    private List<(string Text, bool Match)> Highlight(string name)
    {
        List<(string Text, bool Match)> segments = [];

        string search = _search.Trim();
        string needle = search.Length == 0 ? string.Empty : string.RemoveDiacritics(search);

        if (needle.Length == 0)
        {
            segments.Add((name, false));
            return segments;
        }

        string haystack = string.RemoveDiacritics(name);

        if (haystack.Contains(needle, StringComparison.OrdinalIgnoreCase))
        {
            int cursor = 0;
            while (cursor < name.Length)
            {
                int match = haystack.IndexOf(needle, cursor, StringComparison.OrdinalIgnoreCase);

                if (match < 0)
                {
                    segments.Add((name[cursor..], false));
                    break;
                }

                if (match > cursor)
                    segments.Add((name[cursor..match], false));

                int end = match + needle.Length;
                segments.Add((name[match..end], true));

                cursor = end;
            }

            return segments;
        }

        if (TryHighlightInitials(name, haystack, needle, segments))
            return segments;

        segments.Add((name, false));
        return segments;
    }

    private static bool TryHighlightInitials(string name, string haystack, string needle, List<(string Text, bool Match)> segments)
    {
        Span<char> initials = stackalloc char[7];
        Span<int> wordStart = stackalloc int[7];
        int count = 0;

        foreach (Range range in haystack.AsSpan().Split(' '))
        {
            ReadOnlySpan<char> word = haystack.AsSpan()[range];

            if (word.IsEmpty || count == initials.Length)
                continue;

            initials[count] = word[0];
            wordStart[count] = range.Start.Value;
            count++;
        }

        int at = initials[..count].IndexOf(needle, StringComparison.OrdinalIgnoreCase);

        if (at < 0)
            return false;

        int end = at + needle.Length;
        int cursor = 0;

        for (int i = at; i < end; i++)
        {
            int pos = wordStart[i];

            if (pos > cursor)
                segments.Add((name[cursor..pos], false));

            segments.Add((name[pos..(pos + 1)], true));
            cursor = pos + 1;
        }

        if (cursor < name.Length)
            segments.Add((name[cursor..], false));

        return true;
    }

    private static class Keyboard
    {
        internal const string ArrowDown = "ArrowDown";
        internal const string ArrowUp = "ArrowUp";
        internal const string Escape = "Escape";
        internal const string Enter = "Enter";
    }
}
