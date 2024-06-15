using dOSC.Utilities;

namespace dOSC.Drivers.Settings;

public class WebsocketSetting : SettingBase
{
    public bool Enabled { get; set; } = true; 
    public int Port { get; set; } = 60065;
    public string Key { get; set; } = EncryptionHelper.GenerateApiKey();
}