using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSC.Components.UI
{
    public partial class Header
    {
        [Inject] private IJSRuntime? _js { get; set; }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
                _js.InvokeVoidAsync("blazorjs.dragable");

            return base.OnAfterRenderAsync(firstRender);
        }

    }
}
