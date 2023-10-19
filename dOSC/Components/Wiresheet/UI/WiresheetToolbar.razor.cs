using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.UI
{
    public partial class WiresheetToolbar
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
