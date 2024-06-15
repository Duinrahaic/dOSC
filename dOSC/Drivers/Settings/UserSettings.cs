using System.Collections.Generic;
using dOSC.Drivers.Websocket;
using Newtonsoft.Json;

namespace dOSC.Drivers.Settings;

[JsonObject]
public class UserSettings
{
    public WiresheetSetting Wiresheet { get; set; } = new();
    public OSCSetting OSC { get; set; } = new();
    public PulsoidSetting Pulsoid { get; set; } = new();
    public WebsocketSetting Websocket { get; set; } = new();
    public List<SettingBase> GetSettings()
    {
        return new List<SettingBase> { Wiresheet, OSC, Pulsoid , Websocket };
    }
}