// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Unit.Tests.Fixtures;

internal abstract class CountryJson(string filename)
{
    internal string Value { get; } = GetJson(filename);

    private static string GetJson(string filename)
    {
        const string path = "Fixtures/Data";

        return File.ReadAllText(Path.Combine(path, filename));
    }
}
