using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.App;

public partial class AppBody
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}