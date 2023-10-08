using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Inputs
{
    public partial class DropdownOptions
    {
        [Parameter]
        public string Selection { get; set; } = string.Empty;
        [Parameter]
        public List<string> Options { get; set; }
    }
}
