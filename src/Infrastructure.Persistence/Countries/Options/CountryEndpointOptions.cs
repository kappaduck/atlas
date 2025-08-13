// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Countries.Options;

[ExcludeFromCodeCoverage]
internal sealed partial class CountryEndpointOptions
{
    internal const string Section = "countries:endpoints";

    [Required, Url]
    public required string All { get; set; }

    [Required, Url]
    public required string Lookup { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CountryEndpointOptions>;
}
