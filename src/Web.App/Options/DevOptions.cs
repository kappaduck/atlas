// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Web.App.Options;

[ExcludeFromCodeCoverage]
public sealed class DevOptions
{
    internal const string Section = "dev";

    public bool Debug { get; set; }
}
