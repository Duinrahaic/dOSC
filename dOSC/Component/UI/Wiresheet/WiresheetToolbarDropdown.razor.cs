using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.Wiresheet;

public partial class WiresheetToolbarDropdown
{
    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public RenderFragment? ChildContent { get; set; }
}