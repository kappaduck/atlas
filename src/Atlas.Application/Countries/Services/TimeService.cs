// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application.Countries.Services;

[ExcludeFromCodeCoverage]
internal sealed class TimeService : ITimeService
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}
