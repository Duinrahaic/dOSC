using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace dOSCEngine.Services.User
{
    public class XSOverlaySetting : SettingBase
    {
        [JsonIgnore]
        public override string Name { get; set; } = "XSOverlay";
        [JsonIgnore]
        public override string Description { get; set; } = "XSOverlay";
        public override bool IsEnabled { get; set; } = false;
        public override bool IsConfigured { get; set; } = true;
        [JsonIgnore]
        public override SettingType SettingType { get; set; } = SettingType.XSOverlay;
        [Required]
        [Range(1, 65535)]
        public uint Port { get; set; } = 42069;

    }
}
