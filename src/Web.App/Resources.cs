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

    internal const string Atlas = nameof(Atlas);

    internal const string AtlasLogo = nameof(AtlasLogo);

    internal const string GoBack = nameof(GoBack);

    internal const string Navigation = nameof(Navigation);

    internal const string NotFound = nameof(NotFound);

    internal const string NotFoundMessage = nameof(NotFoundMessage);

    internal const string Project = nameof(Project);
}
