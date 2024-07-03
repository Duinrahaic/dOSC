using System.Windows.Forms;
using dOSC.Attributes;
using dOSC.Client.Models.Commands;
using dOSC.Drivers.Hub;
using dOSC.Drivers.OSC;
using dOSC.Drivers.Settings;
using dOSC.Utilities;
using LiteDB;

namespace dOSC.Drivers.VRChat;

public partial class VRChatService : ConnectorBase
{
    private readonly ILogger<VRChatService> _logger;

    public override string Name => "VRChat";
    public override string IconRef => @"/images/VRChat-Logo-500x281.png";    
    public override string Description => "Monitors OSC and Avatar Parameters in VRChat";
    
    private OscService _oscService;
    private VRChatSettings GetConfiguration() => (VRChatSettings) Configuration;

    private HashSet<string> _owners = new();
    
    public VRChatService(IServiceProvider services) : base(services)
    {
        _logger = services.GetService<ILogger<VRChatService>>()!;
        _logger.LogInformation("Initialized VRChatService");
        _oscService = services.GetRequiredService<OscService>();
        Configuration = AppFileSystem.LoadSettings().VRChat;
        foreach (var owner in EndpointHelper.GetEndpoints(this).Select(x=>x.Owner))
        {
            _owners.Add(owner);
            DataWriterService.RegisterOwner(owner,updateHandler: UpdateHandler);
        }

        _owners.Add("VRChat-Avatar Parameters");
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
                _oscService.SendMessage(endpoint.Name, _oscService.FormatValue(value.RawValue));
            }
        }
        
    }
    
    
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // Ignore if not enabled
        if (!Configuration.Enabled)
        {
            return;
        }
        Enabled = true;
        HubService.Log(new()
        {
            Origin = "VRChatService",
            Message = "VRChat OSC Service Started",
            Level = DoscLogLevel.Info,
        });
        _oscService.OnOscMessageReceived += OnOscMessageReceived;

        await Task.CompletedTask;
    }
    
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Enabled = false;
        HubService.Log(new()
        {
            Origin = "VRChatService",
            Message = "VRChat OSC Service Stopped",
            Level = DoscLogLevel.Info,
        });
        _oscService.OnOscMessageReceived -= OnOscMessageReceived;

        await Task.CompletedTask;
    }

    private void OnOscMessageReceived(OSCSubscriptionEvent e)
    { 
        var endpoint = HubService.GetEndpoints().FirstOrDefault(x=> _owners.Contains(x.Owner) && x.Name == e.Message.Address);
        if (endpoint != null)
        {
            var value = endpoint.ToDataEndpointValue();
            value.Value = e.Message.Arguments.First().ToString();
            HubService.UpdateEndpointValue(value);
            EndpointHelper.TryUpdateEndpointProperty(this, endpoint, e.EndpointToBsonValue(endpoint));
            
            
        }
        else
        {
            endpoint = e.GetEndpoint("VRChat-Avatar Parameters");
            HubService.RegisterEndpoint(endpoint);
        }
    }
    
    
    
    

    public override void StopService()
    {
        if (Running)
        {
            Running = false;
            _logger.LogInformation("VRChatService stopped");

        }
    }

    public override void StartService()
    {
        if (!Running)
        {
            StartAsync(CancellationToken.None);
        }
    }

    protected void Dispose()
    {
        Unwatch();
    }

}