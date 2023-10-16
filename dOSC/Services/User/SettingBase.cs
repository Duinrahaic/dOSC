using Newtonsoft.Json;
namespace dOSC.Services.User
{
    public abstract class SettingBase
    {
        [JsonIgnore]
        public virtual string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual string Description { get; set; } = string.Empty;
        public virtual bool IsEnabled { get; set; } = true;
        public virtual bool IsConfigured { get; set; } = true;
        [JsonIgnore]
        public virtual SettingType SettingType { get; set; } = SettingType.Unknown;
    }
}
