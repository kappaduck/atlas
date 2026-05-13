// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Storage;

internal sealed class DailyCountryStorage(ILocalStorage storage) : DailyLocalStorage(Key, storage)
{
    internal const string Key = "country";
}
