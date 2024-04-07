using dOSC.Shared.Models.Settings;
using dOSCEngine.Services.User;
using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Form.SettingsPages
{
    public partial class DOSCSettings
    {
        [Parameter]
        public dOSCSetting? Setting { get; set; }
    }
}
