// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application;
using Microsoft.Extensions.Localization;

namespace Unit.Tests.Fixtures;

public sealed class LocalizerFixture
{
    internal IStringLocalizer<Resources> Countries { get; } = Substitute.For<IStringLocalizer<Resources>>();
}
