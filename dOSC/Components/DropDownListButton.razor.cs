using Microsoft.AspNetCore.Components;

namespace dOSC.Components
{
    public partial class DropDownListButton
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
