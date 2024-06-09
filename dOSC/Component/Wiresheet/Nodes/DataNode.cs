using dOSC.Client.Models.Commands;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes;

public abstract class DataNode: WiresheetNode
{
    public delegate void EndpointChanged(DataEndpoint newEndpoint);
    public event EndpointChanged? OnEndpointChanged;
    
    public DataNode() : base()
    {
        this.SilentSetValue(0.0);
    }
    
    public override string NodeName => "Data Node";
    public override NodeCategory Category => NodeCategory.Data;
    
    private DataEndpoint _endpoint = new();
    
    [LiveSerialize] public DataEndpoint? EndPoint { 
        get => _endpoint;
        set
        {
            if(value != null)
            {
                SetEndpoint(value);
            }
            
            
            
        }
    }
    
    protected void SetEndpoint(DataEndpoint endpoint)
    {
        _endpoint = endpoint;
        OnEndpointChanged?.Invoke(endpoint);
    }
    
    
    
}