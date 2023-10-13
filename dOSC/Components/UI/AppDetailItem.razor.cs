using Microsoft.AspNetCore.Components;

namespace dOSC.Components.UI
{
    public partial class AppDetailItem
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
