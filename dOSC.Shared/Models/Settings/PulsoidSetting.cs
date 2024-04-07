using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace dOSCEngine.Services.User
{
    public class PulsoidSetting : SettingBase
    {
        [JsonIgnore]
        public override string Name { get; set; } = "Pulsoid";
        [JsonIgnore]
        public override string Description { get; set; } = "Heart Rate Monitoring";
        public override bool IsEnabled { get; set; } = false;
        public override bool IsConfigured { get; set; } = true;
        [JsonIgnore]
        public override SettingType SettingType { get; set; } = SettingType.Pulsoid;
        [Required]
        public string AccessToken { get; set; } = string.Empty;

    }
}
