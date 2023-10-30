using dOSCEngine.Services.User;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Form.SettingsPages
{
    public partial class DOSCSettings
    {
        [Parameter]
        public dOSCSetting? Setting { get; set; }
    }
}
