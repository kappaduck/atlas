// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Changelog;

[ExcludeFromCodeCoverage]
public sealed partial class ChangelogOptions
{
    internal const string Section = "project:changelog";

    [Required, Url]
    public required string Url { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<ChangelogOptions>;
}
