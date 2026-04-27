// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Web.App.Dialogs;

public abstract class DialogBase : ComponentBase
{
    protected ElementReference Element { get; set; }

    [Inject]
    private IJSInProcessRuntime JsRuntime { get; set; } = default!;

    protected void Close() => JsRuntime.InvokeVoid("close", Element);

    protected void Show() => JsRuntime.InvokeVoid("show", Element);

    protected void ScrollContentToTop(string css) => JsRuntime.InvokeVoid("scroll", Element, css);
}
