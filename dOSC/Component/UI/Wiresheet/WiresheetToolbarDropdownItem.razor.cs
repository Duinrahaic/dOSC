using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.Wiresheet;

public partial class WiresheetToolbarDropdownItem
{
    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public EventCallback OnClick { get; set; }

    private async Task Clicked()
    {
        await OnClick.InvokeAsync();
    }
}