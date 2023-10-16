using dOSC.Pages;
using Newtonsoft.Json;

namespace dOSC.Services.User
{
    [JsonObject]
    public class UserSettings
    {
        public dOSCSetting dOSC { get; set; } = new();
        public OSCSetting OSC { get; set; } = new();

        public List<SettingBase> GetSettings()
        {
            return new() { dOSC, OSC };
        }
    }
}
