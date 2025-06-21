// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Web.App.Services;

[ExcludeFromCodeCoverage]
internal sealed class TimeService : ITimeService
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}
