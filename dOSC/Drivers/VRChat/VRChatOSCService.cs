using dOSC.Attributes;
using dOSC.Client.Models.Commands;
using dOSC.Drivers.OSC;
using dOSC.Utilities;
using LiteDB;

namespace dOSC.Drivers.VRChat;

public partial class VRChatOSCService : ConnectorBase
{
    private OscService _oscService;
    
    public VRChatOSCService(IServiceProvider services) : base(services)
    {
        _oscService = services.GetRequiredService<OscService>();
        Configuration = AppFileSystem.LoadSettings().VRChat;

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
}