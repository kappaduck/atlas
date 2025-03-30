// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Prometheus.Patch;

internal interface IPatch<T> where T : allows ref struct
{
    T ApplyTo(T target);
}
