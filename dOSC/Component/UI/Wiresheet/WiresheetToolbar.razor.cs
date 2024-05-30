using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.Wiresheet;

public partial class WiresheetToolbar
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}