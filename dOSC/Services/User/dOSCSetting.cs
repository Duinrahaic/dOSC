using Newtonsoft.Json;

namespace dOSC.Services.User
{
    public class dOSCSetting : SettingBase
    {
        [JsonIgnore]
        public override string Name { get; set; } = "General";
        [JsonIgnore]
        public override string Description { get; set; } = "dOSC Settings";
        public override bool IsEnabled { get; set; } = true;
        public override bool IsConfigured { get; set; } = true;
        [JsonIgnore]
        public override SettingType SettingType { get; set; } = SettingType.dOSC;

    }
}
