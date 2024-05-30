using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.Wiresheet;

public partial class WiresheetToolbarDropdownSubmenuItem
{
    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public RenderFragment? ChildContent { get; set; }
}