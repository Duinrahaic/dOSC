using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components
{
    public partial class AppBody
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
