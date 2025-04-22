// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Infrastructure.Json;
using Microsoft.Extensions.Logging;
using Prometheus.Countries.Dto;
using Prometheus.Countries.Mappers;
using Prometheus.Countries.Options;
using Prometheus.Countries.Providers;
using Prometheus.Files;

namespace Prometheus.Countries;

internal sealed partial class CountryMigration(
    ICountryProvider provider,
    IJsonFileWriter writer,
    ILogger<CountryMigration> logger,
    CountryFilterOptions options) : IMigration
{
    public string Name { get; } = "Countries";

    public async Task MigrateAsync(string path, CancellationToken cancellationToken)
    {
        CountryDto[] dto = await provider.GetAllAsync(cancellationToken).ConfigureAwait(false);

        if (dto.Length == 0)
            return;

        Country[] countries = dto.AsDomain(options.Languages, options.ExcludedCountries);
        CountryLookup[] countryLookups = countries.AsLookups();

        MigratingCountries(countries.Length, DataJsonPaths.Countries);
        await writer.WriteToAsync($"{path}/{DataJsonPaths.Countries}", countries, CountryJsonContext.Default.CountryArray, cancellationToken).ConfigureAwait(false);

        MigratingCountriesForSearch(countryLookups.Length, DataJsonPaths.LookupCountries);
        await writer.WriteToAsync($"{path}/{DataJsonPaths.LookupCountries}", countryLookups, CountryJsonContext.Default.CountryLookupArray, cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(LogLevel.Information, "Migrating {length} country to {jsonFile}")]
    private partial void MigratingCountries(int length, string jsonFile);

    [LoggerMessage(LogLevel.Information, "Migrating {length} country for search to {jsonFile}")]
    private partial void MigratingCountriesForSearch(int length, string jsonFile);
}
