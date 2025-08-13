// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Countries.Options;

[ExcludeFromCodeCoverage]
internal sealed partial class ExcludedCountriesOptions
{
    internal const string Section = "countries";

    [Required]
    public required IEnumerable<string> Excluded { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<ExcludedCountriesOptions>;
}
