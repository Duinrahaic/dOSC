using dOSC.Shared.Models.Settings;
using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Form.SettingsPages;

public partial class DOSCSettings
{
    [Parameter] public dOSCSetting? Setting { get; set; }
}