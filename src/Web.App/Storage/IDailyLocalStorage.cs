// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Web.App.Storage;

public interface IDailyLocalStorage
{
    void Abandon();

    void Add(GuessedResponse guess);

    (IEnumerable<GuessedResponse> Guesses, bool Abandon) Get();
}
