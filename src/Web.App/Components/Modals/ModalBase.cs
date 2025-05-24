// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Web.App.Components.Modals;

public class ModalBase : ComponentBase, IDisposable
{
    private bool _disposed;
    private IJSInProcessObjectReference? _module;

    [Inject]
    private IJSInProcessRuntime JsRuntime { get; set; } = default!;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _module = await JsRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "/scripts/modal.js");
    }

    protected void Close(ElementReference dialog) => _module?.InvokeVoid("closeModal", dialog);

    protected void CloseModalOnClickOutside(ElementReference dialog) => _module?.InvokeVoid("closeModalOnClickOutside", dialog);

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _module?.Dispose();
            _module = null;
        }

        _disposed = true;
    }

    protected void ScrollContentToTop(ElementReference dialog) => _module?.InvokeVoid("scrollContentToTop", dialog);

    protected void Show(ElementReference dialog) => _module?.InvokeVoid("showModal", dialog);
}
