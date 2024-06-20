using System;
using System.Collections.Generic;
using CoreOSC;
using dOSC.Attributes;
using dOSC.Client.Models.Commands;
using dOSC.Drivers.Settings;
using dOSC.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;
using dOSC.Drivers.Hub;
using LiteDB;

namespace dOSC.Drivers.OSC;

public partial class OscService : ConnectorBase
{
    public delegate void OscSubscriptionEventHandler(OSCSubscriptionEvent e);
    public event OscSubscriptionEventHandler? OnOscMessageReceived;

    private readonly ILogger<OscService> _logger;
    private UDPDuplex _duplex;
    private UDPListener _listener;
    private UDPSender _sender;
    private OSCSetting GetConfiguration() => (OSCSetting) Configuration;
    private CancellationTokenSource _cts;
    public static int GetDefaultListeningPort() => new OSCSetting().ListeningPort;
    private int _listingPort = 9000;
    public int ListeningPort
    {
        get => _listingPort;
        set
        {
            if (_listingPort != value)
            {
                _listingPort = value;
                var configuration = GetConfiguration();
                configuration.ListeningPort = value;
                SaveConfiguration(Configuration);
            }
        }
    }
    
    public override bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                var config = GetConfiguration();
                config.Enabled = value;
                SaveConfiguration(config);
                if (_enabled)
                {
                    StartService();
                }
                else
                {
                    StopService();
                }
            }
        }
    }
    
    public static int GetDefaultSendingPort() => new OSCSetting().SendingPort;
    private int _sendingPort = 9001;
    public int SendingPort
    {
        get => _sendingPort;
        set
        {
            if (_sendingPort != value)
            {
                _sendingPort = value;
                var configuration = GetConfiguration();
                configuration.SendingPort = value;
                SaveConfiguration(Configuration);
            }
        }
    }

    private readonly HashSet<string> DiscoveredParameters = new();
    
    
    public OscService(IServiceProvider services):base(services)
    {
        _logger = services.GetService<ILogger<OscService>>()!;
        _logger.LogInformation("Initialized OSCService");
        
        Configuration = AppFileSystem.LoadSettings().OSC;
        var configuration = GetConfiguration();
        ListeningPort = configuration.ListeningPort;
        SendingPort = configuration.SendingPort;

        foreach (var owner in EndpointHelper.GetEndpoints(this).Select(x=>x.Owner))
        {
            DataWriterService.RegisterOwner(owner,updateHandler: UpdateHandler);
        }
    }

    private void UpdateHandler(DataEndpoint endpoint, BsonValue value)
    {
        if (EndpointHelper.TryUpdateEndpointProperty(this, endpoint, value))
        {
            var ep = EndpointHelper.GetCurrentEndpoint(this, endpoint.Name);
            if (ep != null)
            {
                HubService.UpdateEndpointValue(ep.ToDataEndpointValue());
                var epv = EndpointHelper.GetEndpointPropertyValue(this, endpoint.Name);
                SendMessage(endpoint.Name, FormatValue(value.RawValue));
            }
        }
        
    }

    private object? FormatValue(object value)
    {
        if (value is double dbl)
        {
            return Math.Clamp(dbl, -1, 1);
        }
        else if (value is int i)
        {
            return  Math.Clamp(i, -255, 255);
        }
        else if (value is bool bol)
        {
            return bol ? 1 : 0;
        }
        return null;
    }
    public override string Name => "OSC";
    public override string Description => "A protocol for networking computers and devices";

    private async Task Run(CancellationToken stoppingToken)
    {
        Running = true;
        HandleOscPacket callback = delegate(OscPacket packet)
        {
            var messageReceived = (OscMessage)packet;

            if (messageReceived != null)
            {
                var endpoints = HubService.GetEndpointsByAddress(messageReceived.Address);
                
                
                if (endpoints.Any())
                {
                    var endpoint = endpoints.First();
                    var value = endpoint.ToDataEndpointValue();
                    value.Value = messageReceived.Arguments.First().ToString();
                    HubService.UpdateEndpointValue(value);
                } 
                
                DiscoveredParameters.Add(messageReceived.Address);
                OnOscMessageReceived?.Invoke(new OSCSubscriptionEvent(messageReceived.Address,
                    messageReceived.Arguments));
            }
        };

        try
        {
            _logger.LogInformation($"OSCService started at Listing on {_listingPort} and Sending on {_sendingPort}");
            HubService.Log(new()
            {
                Origin = "OSC",
                Message = $"OSC started at Listing on {_listingPort} and Sending on {_sendingPort}",
                Level = DoscLogLevel.Info,
            });
            _sender = new UDPSender("127.0.0.1", _sendingPort);
            _listener = new UDPListener(_listingPort, callback);
            //_duplex = new UDPDuplex("127.0.0.1",_listingPort,_sendingPort, callback);
            await Task.Delay(Timeout.Infinite);
        }
        catch(Exception ex)
        {
            _logger.LogError($"OSC Failed: {ex.Message}");
            HubService.Log(new()
            {
                Origin = "OSC",
                Message = $"OSC failed to start: {ex.Message}",
                Level = DoscLogLevel.Info,
                Details = ex.StackTrace
            });
        }

        Running = false;
    }

    
    public override void StopService()
    {
        if (Running)
        {
            Running = false;
            _sender.Close();
            _listener.Close();
            _logger.LogInformation("OSCService stopped");
        }
    }

    public override void StartService()
    {
        if (!Running)
        {
            StartAsync(CancellationToken.None);
        }
    }

    private void SendMessage(string address, params object?[]? args)
    {
        if (args == null)
            return;
        var message = new OscMessage(address, args);
        try
        {
            _sender.Send(message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while sending OSC message: {ex}");
        }
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // Ignore if not enabled
        if (!Configuration.Enabled)
        {
            return;
        }
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        HubService.Log(new()
        {
            Origin = "OSC",
            Message = "OSC Service Started",
            Level = DoscLogLevel.Info,
        });
        Task.Run(() => Run(_cts.Token), _cts.Token);
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Running)
        {
            HubService.Log(new()
            {
                Origin = "OSC",
                Message = "OSC Service Stopped",
                Level = DoscLogLevel.Info,
            });
            Running = false;
        }
    }
}