using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components.Inputs
{
    public partial class DropdownOptions
    {
        [Parameter]
        public string Selection { get; set; } = string.Empty;
        [Parameter]
        public List<string> Options { get; set; }
    }
}
