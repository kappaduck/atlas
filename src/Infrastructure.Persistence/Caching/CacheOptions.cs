// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence.Caching;

[ExcludeFromCodeCoverage]
internal sealed partial class CacheOptions
{
    internal const string Section = "cache";

    [Required]
    public required int ExpirationTimeInMinutes { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CacheOptions>;
}
