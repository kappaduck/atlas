// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Infrastructure.Json;
using NSubstitute.ReturnsExtensions;
using Prometheus.Files;
using Prometheus.Files.Options;

namespace Unit.Tests.Prometheus.Files;

internal sealed class DataDirectoryTests
{
    private readonly string _expectedPath;
    private readonly IDirectory _directory = Substitute.For<IDirectory>();
    private readonly DataPathOptions _options = new()
    {
        Root = "root",
        Output = "output"
    };

    private readonly DataDirectory _dataDirectory;

    public DataDirectoryTests()
    {
        _expectedPath = Path.Combine(_options.Root, _options.Output, DataJsonPaths.BaseDirectory);

        _dataDirectory = new DataDirectory(_directory, _options);
    }

    [Test]
    public void CreateShouldGetTheRoot()
    {
        _dataDirectory.Create();

        _directory.Received(1).GetRootPath(_options.Root);
    }

    [Test]
    public void CreateShouldCreateTheFolder()
    {
        _directory.GetRootPath(_options.Root).Returns(_options.Root);

        _dataDirectory.Create();

        _directory.Received(1).Create(_expectedPath);
    }

    [Test]
    public async Task CreateShouldReturnThePath()
    {
        _directory.GetRootPath(_options.Root).Returns(_options.Root);
        _directory.Create(_expectedPath).Returns(_expectedPath);

        string? path = _dataDirectory.Create();

        await Assert.That(path).IsEqualTo(_expectedPath);
    }

    [Test]
    public async Task CreateShouldReturnNullWhenDoesNotFoundRootPath()
    {
        _directory.GetRootPath(_options.Root).ReturnsNull();

        string? path = _dataDirectory.Create();

        await Assert.That(path).IsNull();
    }

    [Test]
    public void CreateShouldNotCreateTheDirectoryWhenDoesNotFoundRootPath()
    {
        _directory.GetRootPath(_options.Root).ReturnsNull();

        _dataDirectory.Create();

        _directory.DidNotReceive().Create(_expectedPath);
    }
}
