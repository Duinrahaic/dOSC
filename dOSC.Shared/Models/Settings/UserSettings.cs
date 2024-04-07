using System.Collections.Generic;
using Newtonsoft.Json;

namespace dOSC.Shared.Models.Settings;

[JsonObject]
public class UserSettings
{
    public dOSCSetting dOSC { get; set; } = new();
    public OSCSetting OSC { get; set; } = new();
    public PulsoidSetting Pulsoid { get; set; } = new();

    public List<SettingBase> GetSettings()
    {
        return new List<SettingBase> { dOSC, OSC, Pulsoid };
    }
}