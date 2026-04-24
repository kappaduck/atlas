// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog;
using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;

namespace Web.App.Components.Settings;

public sealed partial class ChangelogSection(IChangelogService service) : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private MarkupString _changelog;
    private bool _isLoading;

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        string? content = await service.GetAsync(_cts.Token);
        _changelog = GenerateChangelog(content);

        _isLoading = false;
    }

    private static MarkupString GenerateChangelog(string? content)
    {
        if (string.IsNullOrEmpty(content))
            return new MarkupString("No changelog available.");

        MarkdownDocument document = Markdown.Parse(content);

        HtmlAttributes versionAttributes = new();
        versionAttributes.AddClass("version");

        HtmlAttributes issuesAttributes = new();
        issuesAttributes.AddClass("issues");

        HtmlAttributes issueItemAttributes = new();
        issueItemAttributes.AddClass("issue-item");

        HtmlAttributes linkAttributes = new();
        linkAttributes.AddProperty("target", "_blank");
        linkAttributes.AddClass("issue-link");

        foreach (MarkdownObject descendant in document.Descendants())
        {
            if (descendant is HeadingBlock { Level: 1 } or ParagraphBlock)
                document.Remove((Block)descendant);

            if (descendant is HeadingBlock { Level: 2 })
                descendant.SetAttributes(versionAttributes);

            if (descendant is HeadingBlock { Level: 3 } section)
            {
                HtmlAttributes sectionAttributes = new();
                sectionAttributes.AddClass($"section {GetSectionCss(section.Inline!.FirstChild!.ToString())}");

                section.SetAttributes(sectionAttributes);
            }

            if (descendant is ListBlock)
                descendant.SetAttributes(issuesAttributes);

            if (descendant is ListItemBlock)
                descendant.SetAttributes(issueItemAttributes);

            if (descendant is LinkInline)
                descendant.SetAttributes(linkAttributes);
        }

        return new MarkupString(document.ToHtml());

        static string GetSectionCss(string? sectionName) => sectionName switch
        {
            "Added" => "added",
            "Changed" => "changed",
            "Fixed" => "fixed",
            _ => string.Empty
        };
    }
}
