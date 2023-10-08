using Microsoft.AspNetCore.Components;

namespace dOSC.Components
{
    public partial class DropDownListButtonItem
    {
        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public string ActionName { get; set; }
    }
}
