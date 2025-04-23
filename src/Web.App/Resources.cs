// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Web.App;

[ExcludeFromCodeCoverage]
internal sealed class Resources
{
    private Resources()
    {
    }

    internal const string GoBack = nameof(GoBack);

    internal const string NotFound = nameof(NotFound);

    internal const string NotFoundMessage = nameof(NotFoundMessage);
}
