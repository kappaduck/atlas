// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Countries.Services;

internal interface ITimeService
{
    DateOnly Today { get; }
}
