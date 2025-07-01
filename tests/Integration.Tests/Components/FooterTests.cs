// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using Web.App.Components;
using Web.App.Options;

namespace Integration.Tests.Components;

internal sealed class FooterTests : BunitContext
{
    public FooterTests()
    {
        ProjectOptions project = new()
        {
            BugUrl = "https://bug.com",
            FeatureUrl = "https://feature.com",
            Url = "https://atlas.com",
            Version = "1.0.0"
        };

        CompanyOptions company = new()
        {
            Name = "KappaDuck",
            Url = "https://kappaduck.com"
        };

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
}
