using CoreOSC;
using dOSC.Shared.Models.Settings;
using dOSC.Shared.Utilities;
using dOSCEngine.Services.Connectors;

namespace dOSC.Client.Services.Connectors.Hub.OSC
{
    public partial class OSCService : ConnectorBase
    {
        public delegate void OSCSubscriptionEventHandler(OSCSubscriptionEvent e);
        public event OSCSubscriptionEventHandler? OnOSCMessageRecieved;

        public override string ServiceName => "OSC";
        public override string Description => "A protocol for networking computers and devices";

        private readonly ILogger<OSCService> _logger;
        private System.Timers.Timer? _refreshTimer;
        private UDPSender? _sender;
        private UDPListener? _receiver;
        private int TCPPort = 9000;
        private int UDPPort = 9001;
        private OSCSetting Setting;

        public OSCService(IServiceProvider services)
        {
            _logger = services.GetService<ILogger<OSCService>>()!;
            _logger.LogInformation("Initialized OSCService");
            StartService();
        }

        private HashSet<string> DiscoveredParameters = new();

        public override void LoadSetting()
        {
            Setting = (dOSCFileSystem.LoadSettings() ?? new()).OSC;
        }
        public override SettingBase GetSetting() => Setting ?? new OSCSetting();

        public override void StartService()
        {
            if (!isRunning())
            {

                Running = true;
            }
        }

        public override void StopService()
        {
           
            if( isRunning()) 
                Running = false;
            _logger.LogInformation($"OSCService stopped");
        }


        
        public void SendMessage(string Address, params object[] args)
        {
            //var message = new OscMessage(Address, args);
            if(_sender != null)
            {
                try
                {

                    
                    //_sender.Send(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while sending OSC message: {ex}");
                }
            }
            else
            {
                _logger.LogWarning("Cannot send OSC message. Sender is null.");
            }
            
        }
    }
}
