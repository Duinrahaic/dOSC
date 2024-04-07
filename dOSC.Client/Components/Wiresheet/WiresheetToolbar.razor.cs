using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Wiresheet;

public partial class WiresheetToolbar
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}