// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Prometheus.Files.Options;

internal sealed partial class DataPathOptions
{
    internal const string Section = "data";

    [Required]
    public required string Root { get; set; }

    [Required]
    public required string Output { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<DataPathOptions>;
}
