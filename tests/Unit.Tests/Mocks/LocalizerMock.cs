// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application;
using Microsoft.Extensions.Localization;

namespace Unit.Tests.Mocks;

public sealed class LocalizerMock
{
    internal IStringLocalizer<Translations> Application { get; } = Substitute.For<IStringLocalizer<Translations>>();
}
