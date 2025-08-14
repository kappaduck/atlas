// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Web.App.Options;

[ExcludeFromCodeCoverage]
internal sealed class FeatureOptions
{
    internal const string Section = "features";

    public bool DevMode { get; set; }
}
