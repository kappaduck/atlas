// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Web.App.Options;

[ExcludeFromCodeCoverage]
internal sealed partial class ProjectOptions
{
    internal const string Section = "project";

    [Required]
    public required string Version { get; set; }

    [Required, Url]
    public required string Url { get; set; }

    [Required, Url]
    public required string BugUrl { get; set; }

    [Required, Url]
    public required string FeatureUrl { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<ProjectOptions>;
}
