using Newtonsoft.Json;

namespace dOSC.Shared.Models.Settings;

public abstract class SettingBase
{
    [JsonIgnore] public virtual string Name { get; set; } = string.Empty;

    [JsonIgnore] public virtual string Description { get; set; } = string.Empty;

    public virtual bool IsEnabled { get; set; } = false;
    public virtual bool IsConfigured { get; set; } = false;

    [JsonIgnore] public virtual SettingType SettingType { get; set; } = SettingType.Unknown;
}