using dOSCEngine.Services.User;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components.Form.SettingsPages
{
    public partial class DOSCSettings
    {
        [Parameter]
        public dOSCSetting? Setting { get; set; }
    }
}
