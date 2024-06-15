using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace dOSC.Drivers.Settings;

public class OSCSetting : SettingBase
{
    public string Host { get; set; } = "127.0.0.1";

    [Range(1, 65535)] 
    public int TcpPort { get; set; } = 9000;

    [Range(1, 65535)] 
    public int UdpPort { get; set; } = 9001;
}