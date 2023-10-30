using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components.Wiresheet.UI
{
    public partial class WiresheetToolbarDropdownItem
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        private async Task Clicked()
        {
            await OnClick.InvokeAsync();
        }
    }
}
