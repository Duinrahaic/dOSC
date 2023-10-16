using dOSC.Services.User;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Form.SettingsPages
{
    public partial class OSCSettings
    {
        [Parameter]
        public OSCSetting? Setting { get; set; }
    }
}
