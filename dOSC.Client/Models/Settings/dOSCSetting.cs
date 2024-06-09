using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace dOSC.Shared.Models.Settings;

public class dOSCSetting : SettingBase
{
    [JsonIgnore] public override string Name { get; set; } = "General";

    [JsonIgnore] public override string Description { get; set; } = "dOSC Settings";

    public override bool IsEnabled { get; set; } = true;
    public override bool IsConfigured { get; set; } = true;

    [JsonIgnore] public override SettingType SettingType { get; set; } = SettingType.dOSC;

    public string AuthorName { get; set; } = string.Empty;


    [Required] [Range(1, 65535)] public int WebServerPort { get; set; } = 5001;

    public bool FindFirstAvailablePort { get; set; } = false;

    [Required] [Range(1, 65535)] public int HubServerPort { get; set; } = 5002;

    public static int FreeTcpPort()
    {
        var l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        var port = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return port;
    }

    public int GetWebServerPort()
    {
#if DEBUG
        return 5231;
#endif
        if (FindFirstAvailablePort) return FreeTcpPort();
        return WebServerPort;
    }

    public int GetHubServerPort()
    {
#if DEBUG
        return 5232;
#endif
        return HubServerPort;
    }
}