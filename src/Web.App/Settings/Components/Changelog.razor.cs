// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Queries;
using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Mediator;
using Microsoft.AspNetCore.Components;

namespace Web.App.Settings.Components;

public sealed partial class Changelog(IMediator mediator)
{
    private string? _changelog;
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _changelog = await mediator.Send(new GetChangelog.Query());

        _isLoading = false;
        StateHasChanged();
    }

    private MarkupString ChangelogContent()
    {
        if (string.IsNullOrEmpty(_changelog))
            return new MarkupString("No changelog available.");

        MarkdownDocument document = Markdown.Parse(_changelog);

        HtmlAttributes linkAttributes = new();
        linkAttributes.AddProperty("target", "_blank");
        linkAttributes.AddClass("link");

        HtmlAttributes headingAttributes = new();
        headingAttributes.AddClass("version");

        HtmlAttributes listAttributes = new();
        listAttributes.AddClass("section");

        foreach (MarkdownObject descendant in document.Descendants())
        {
            if (descendant is HeadingBlock { Level: 2 })
                descendant.SetAttributes(headingAttributes);

            if (descendant is LinkInline)
                descendant.SetAttributes(linkAttributes);

            if (descendant is ListBlock)
                descendant.SetAttributes(listAttributes);
        }

        return new MarkupString(document.ToHtml());
    }
}
