using CoreOSC;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Services.User;
using dOSCEngine.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace dOSCEngine.Services.Connectors.OSC
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
            Setting = (FileSystem.LoadSettings() ?? new()).OSC;
        }
        public override SettingBase GetSetting() => Setting ?? new OSCSetting();

        public override void StartService()
        {
            if (!isRunning())
            {
                int tcpPort = TCPPort;
                int udpPort = UDPPort;
                HandleOscPacket callback = delegate (OscPacket packet)
                {
                    OscMessage messageRecieved = (OscMessage)packet;

                    //_logger.LogInformation($"Received OSC packet {messageRecieved.Address} [{messageRecieved.Arguments.FirstOrDefault() ?? "Empty"}]");
                    if (messageRecieved != null)
                    {
                        DiscoveredParameters.Add(messageRecieved.Address);
                        OnOSCMessageRecieved?.Invoke(new OSCSubscriptionEvent(messageRecieved.Address, messageRecieved.Arguments));
                    }
                };
                _sender = new UDPSender("127.0.0.1", tcpPort);
                _receiver = new UDPListener(udpPort, callback);
                _logger.LogInformation($"OSCService started at TCP {tcpPort} and UDP {udpPort}");
                Running = true;
            }
        }

        public override void StopService()
        {
            if(_sender != null)
                _sender.Close();
            if(_receiver != null)
                _receiver.Close();
            if( isRunning()) 
                Running = false;
            _logger.LogInformation($"OSCService stopped");
        }


        
        public void SendMessage(string Address, params object[] args)
        {
            var message = new OscMessage(Address, args);
            if(_sender != null)
            {
                try
                {

                    
                    _sender.Send(message);
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
