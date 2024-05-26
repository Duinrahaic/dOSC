using dOSC.Shared.Models.Settings;
using dOSC.Shared.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace dOSC.Client.Services.Connectors.Hub.OSC;

public partial class OSCService : ConnectorBase
{
    public delegate void OSCSubscriptionEventHandler(OSCSubscriptionEvent e);

    private readonly ILogger<OSCService> _logger;
    private Timer? _refreshTimer;

    private HashSet<string> DiscoveredParameters = new();
    private OSCSetting Setting;
    private int TCPPort = 9000;
    private int UDPPort = 9001;

    public OSCService(IServiceProvider services)
    {
        _logger = services.GetService<ILogger<OSCService>>()!;
        _logger.LogInformation("Initialized OSCService");
        //StartService();
    }

    public override string ServiceName => "OSC";
    public override string Description => "A protocol for networking computers and devices";
    public event OSCSubscriptionEventHandler? OnOSCMessageRecieved;

    public override void LoadSetting()
    {
        Setting = (dOSCFileSystem.LoadSettings() ?? new UserSettings()).OSC;
    }

    public override SettingBase GetSetting()
    {
        return Setting ?? new OSCSetting();
    }

    public override void StartService()
    {
        if (!isRunning()) Running = true;
    }

    public override void StopService()
    {
        if (isRunning())
            Running = false;
        _logger.LogInformation("OSCService stopped");
    }


    public void SendMessage(string Address, params object[] args)
    {

    }
}