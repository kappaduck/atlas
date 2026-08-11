// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog;
using Markdig;
using Markdig.Helpers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Web.App.Settings.Components;

public sealed partial class ChangelogSection(IChangelogService service)
{
    private MarkupString _changelog;
    private bool _isLoading;
    private bool _hasError;

    [Parameter, EditorRequired]
    public required CancellationToken CancellationToken { get; init; }

    protected override Task OnInitializedAsync() => FetchChangelogAsync();

    private static MarkupString GenerateChangelog(string content)
    {
        MarkdownDocument document = Markdown.Parse(content);

        HtmlAttributes versionAttributes = new();
        versionAttributes.AddClass("version");

        HtmlAttributes issuesAttributes = new();
        issuesAttributes.AddClass("issues");

        HtmlAttributes issueItemAttributes = new();
        issueItemAttributes.AddClass("issue-item");

        HtmlAttributes linkAttributes = new();
        linkAttributes.AddProperty("target", "_blank");
        linkAttributes.AddClass("quack-link");

        foreach (MarkdownObject descendant in document.Descendants())
        {
            if (descendant is HeadingBlock { Level: 1 } or ParagraphBlock)
                document.Remove((Block)descendant);

            if (descendant is HeadingBlock { Level: 2 } version)
            {
                version.SetAttributes(versionAttributes);

                if (DateOnly.TryParseExact(version.Inline!.FirstChild!.ToString(), "yyyy.MM.dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateOnly date))
                {
                    LiteralInline literal = (version.Inline.FirstChild as LiteralInline)!;
                    literal.Content = new StringSlice(date.ToString("dd MMM yyyy", CultureInfo.CurrentCulture));
                }
            }

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

    private async Task FetchChangelogAsync()
    {
        _hasError = false;
        _isLoading = true;

        try
        {
            string content = await service.GetAsync(CancellationToken);
            _changelog = GenerateChangelog(content);
        }
        catch (HttpRequestException)
        {
            _hasError = true;
        }
        finally
        {
            _isLoading = false;
        }
    }
}
