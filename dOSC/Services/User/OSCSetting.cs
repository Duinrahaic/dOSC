using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace dOSC.Services.User
{
    public class OSCSetting : SettingBase
    {
        [JsonIgnore]
        public override string Name { get; set; } = "OSC";
        [JsonIgnore]
        public override string Description { get; set; } ="Open Sound Control (OSC) is a protocol for networking sound synthesizers, computers, and other multimedia devices for purposes such as musical performance or show control. OSC's advantages include interoperability, accuracy, flexibility, and enhanced organization and documentation.";
        public override bool IsEnabled { get; set; } = true;
        public override bool IsConfigured { get; set; } = true;
        [JsonIgnore]
        public override SettingType SettingType { get; set; } = SettingType.OSC; 
        public string Host { get; set; } = "127.0.0.1";
        [Range(1, 65535)]
        public int TCPPort { get; set; } = 9000;
        [Range(1, 65535)]
        public int UDPPort { get; set; } = 9001;
    }
}
