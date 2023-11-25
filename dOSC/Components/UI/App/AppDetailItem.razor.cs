using Microsoft.AspNetCore.Components;

namespace dOSC.Components.UI.App
{
    public partial class AppDetailItem
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
