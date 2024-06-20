using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using dOSC.Utilities;

namespace dOSC.Drivers.Settings;

public class PulsoidSetting : SettingBase
{
    [Required] public string Key { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true; 
    private void OpenHelp()
    {
        WebUtilities.OpenUrl("https://pulsoid.net/ui/keys");
    }
}