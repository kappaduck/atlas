// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Web.App.Options;

[ExcludeFromCodeCoverage]
internal sealed partial class CompanyOptions
{
    internal const string Section = "project:company";

    [Required]
    public required string Name { get; set; }

    [Required, Url]
    public required string Url { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CompanyOptions>;
}
