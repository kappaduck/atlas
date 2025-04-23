// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using Web.App.Components;
using Web.App.Options;
using Web.App.Services;

namespace Integration.Tests.Components;

internal sealed class FooterTests : BunitContext
{
    private readonly ITimeService _timeService = Substitute.For<ITimeService>();

    public FooterTests()
    {
        ProjectOptions project = new()
        {
            BugUrl = "https://bug.com",
            Url = "https://atlas.com",
            Version = "1.0.0"
        };

        CompanyOptions company = new()
        {
            Name = "KappaDuck",
            Url = "https://kappaduck.com"
        };

        Services.AddTransient(_ => _timeService);
        Services.AddSingleton(project);
        Services.AddSingleton(company);
    }

    [Test]
    public async Task FooterShouldRenderOnce()
    {
        IRenderedComponent<Footer> footer = Render<Footer>();

        footer.Render();
        footer.Render();

        await Assert.That(footer.RenderCount).IsEqualTo(1);
    }

    [Test]
    public async Task FooterShouldDisplayCurrentCopyrightYearWhenStartYearIsEqualToCurrentYear()
    {
        const int currentYear = 2024;
        _timeService.Today.Returns(new DateOnly(currentYear, 1, 1));

        IRenderedComponent<Footer> footer = Render<Footer>();

        IElement element = footer.Find("ul > li:first-child");

        await Assert.That(element.TextContent).IsEqualTo($"© {currentYear}");
    }

    [Test]
    public async Task FooterShouldDisplayRangeOfYearsWhenStartYearIsNotEqualToCurrentYear()
    {
        const int startYear = 2024;
        const int endYear = 2025;
        _timeService.Today.Returns(new DateOnly(endYear, 1, 1));

        IRenderedComponent<Footer> footer = Render<Footer>();

        IElement element = footer.Find("ul > li:first-child");

        await Assert.That(element.TextContent).IsEqualTo($"© {startYear} - {endYear}");
    }
}
