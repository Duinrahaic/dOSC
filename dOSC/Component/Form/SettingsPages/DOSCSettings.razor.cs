using dOSC.Shared.Models.Settings;
using Microsoft.AspNetCore.Components;

namespace dOSC.Component.Form.SettingsPages;

public partial class DOSCSettings
{
    [Parameter] public dOSCSetting? Setting { get; set; }
}