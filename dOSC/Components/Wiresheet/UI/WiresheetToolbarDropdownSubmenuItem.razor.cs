using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.UI
{
    public partial class WiresheetToolbarDropdownSubmenuItem
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
