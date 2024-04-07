using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components
{
    public partial class AppBody
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
