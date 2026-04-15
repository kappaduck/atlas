// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Web.App.Storage;

[ExcludeFromCodeCoverage]
internal sealed class DailyFlagStorage(ILocalStorage storage) : DailyLocalStorage(Key, storage)
{
    internal const string Key = "flag";
}
