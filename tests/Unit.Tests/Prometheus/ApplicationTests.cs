// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute.ReturnsExtensions;
using Prometheus;
using Prometheus.Files;
using App = Prometheus.Application;

namespace Unit.Tests.Prometheus;

internal sealed class ApplicationTests
{
    private readonly IDataDirectory _dataDirectory = Substitute.For<IDataDirectory>();
    private readonly IMigration _migration = Substitute.For<IMigration>();
    private readonly IHostApplicationLifetime _lifetime = Substitute.For<IHostApplicationLifetime>();

    private readonly App _application;

    public ApplicationTests()
    {
        ILogger<App> logger = Substitute.For<ILogger<App>>();

        _application = new App(_dataDirectory, [_migration], _lifetime, logger);
    }

    [Test]
    public async Task StartAsyncShouldCreateTheDataDirectory()
    {
        await _application.StartAsync(CancellationToken.None);

        _dataDirectory.Received(1).Create();
    }

    [Test]
    public async Task StartAsyncShouldStopApplicationWhenDataDirectoryIsNotCreated()
    {
        _dataDirectory.Create().ReturnsNull();

        await _application.StartAsync(CancellationToken.None);

        _lifetime.Received(1).StopApplication();
    }

    [Test]
    public async Task StartAsyncShouldNotMigrateWhenDataDirectoryIsNotCreated()
    {
        _dataDirectory.Create().ReturnsNull();

        await _application.StartAsync(CancellationToken.None);

        await _migration.DidNotReceive().MigrateAsync(Arg.Any<string>(), CancellationToken.None);
    }

    [Test]
    public async Task StartAsyncShouldMigrateWhenDataDirectoryIsCreated()
    {
        _dataDirectory.Create().Returns("path");

        await _application.StartAsync(CancellationToken.None);

        await _migration.Received(1).MigrateAsync("path", CancellationToken.None);
    }

    [Test]
    public async Task StartAsyncShouldStopApplicationWhenFinishedMigration()
    {
        _dataDirectory.Create().Returns("path");

        await _application.StartAsync(CancellationToken.None);

        await _migration.Received(1).MigrateAsync("path", CancellationToken.None);
        _lifetime.Received(1).StopApplication();
    }
}
