using System;
using System.Collections.Generic;
using CoreOSC;
using dOSC.Drivers.Settings;
using dOSC.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace dOSC.Drivers.OSC;

public partial class OSCService : ConnectorBase
{
    public delegate void OSCSubscriptionEventHandler(OSCSubscriptionEvent e);

    private readonly ILogger<OSCService> _logger;
    private UDPListener? _receiver;
    private Timer? _refreshTimer;
    private UDPSender? _sender;
    private readonly int _tcpPort = 9000;
    private readonly int _udpPort = 9001;

    private readonly HashSet<string> DiscoveredParameters = new();
    
    
    private OSCSetting Setting;

    public OSCService(IServiceProvider services):base(services)
    {
        _logger = services.GetService<ILogger<OSCService>>()!;
        _logger.LogInformation("Initialized OSCService");
        StartService();
    }

    public override string ServiceName => "OSC";
    public override string Description => "A protocol for networking computers and devices";
    public event OSCSubscriptionEventHandler? OnOSCMessageRecieved;

    public override void LoadSetting()
    {
        Setting = (AppFileSystem.LoadSettings() ?? new UserSettings()).OSC;
    }

    public override SettingBase GetSetting()
    {
        return Setting ?? new OSCSetting();
    }

    public override void StartService()
    {
        if (!IsRunning())
        {
            var tcpPort = _tcpPort;
            var udpPort = _udpPort;
            HandleOscPacket callback = delegate(OscPacket packet)
            {
                var messageRecieved = (OscMessage)packet;

                //_logger.LogInformation($"Received OSC packet {messageRecieved.Parameter} [{messageRecieved.Arguments.FirstOrDefault() ?? "Empty"}]");
                if (messageRecieved != null)
                {
                    DiscoveredParameters.Add(messageRecieved.Address);
                    OnOSCMessageRecieved?.Invoke(new OSCSubscriptionEvent(messageRecieved.Address,
                        messageRecieved.Arguments));
                }
            };
            _sender = new UDPSender("127.0.0.1", tcpPort);
            try
            {
                _receiver = new UDPListener(udpPort, callback);
                _logger.LogInformation($"OSCService started at TCP {tcpPort} and UDP {udpPort}");
            }
            catch
            {
                _logger.LogError($"Unable to create OSC listener on port {udpPort}");
            }

            Running = true;
        }
    }

    public override void StopService()
    {
        if (_sender != null)
            _sender.Close();
        if (_receiver != null)
            _receiver.Close();
        if (IsRunning())
            Running = false;
        _logger.LogInformation("OSCService stopped");
    }


    public void SendMessage(string Address, params object[] args)
    {
        var message = new OscMessage(Address, args);
        if (_sender != null)
            try
            {
                _sender.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while sending OSC message: {ex}");
            }
        else
            _logger.LogWarning("Cannot send OSC message. Sender is null.");
    }
}