using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Wiresheet;

public partial class WiresheetToolbarDropdownSubmenuItem
{
    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public RenderFragment? ChildContent { get; set; }
}