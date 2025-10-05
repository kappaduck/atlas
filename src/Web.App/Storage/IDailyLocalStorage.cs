// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries;

namespace Web.App.Storage;

public interface IDailyLocalStorage
{
    (GuessedCountryResponse[] Guesses, bool GiveUp) Get(string key);

    void Set(string key, ICollection<GuessedCountryResponse> guesses);

    void Set(string key, bool giveUp);
}
