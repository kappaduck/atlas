// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.CompilerServices;

namespace Atlas.Application.Countries.Responses;

[Union]
public readonly struct GuessedResponse : IUnion
{
    private readonly WrongGuessResponse? _wrong;
    private readonly GoodGuessResponse? _good;

    public GuessedResponse(WrongGuessResponse guess)
    {
        _wrong = guess;

        Success = false;
        Cca2 = guess.Cca2;
    }

    public GuessedResponse(GoodGuessResponse guess)
    {
        _good = guess;

        Success = true;
        Cca2 = guess.Cca2;
    }

    public string Cca2 { get; }

    public bool Success { get; }

    public readonly object? Value => Success ? _good : _wrong;
}
