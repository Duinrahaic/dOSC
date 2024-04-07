using dOSC.Client.Engine.Nodes;
using dOSC.Client.Services.Connectors.Hub.Activity.Pulsoid;
using dOSC.Client.Services.Connectors.Hub.OSC;

namespace dOSC.Client.Services;

public class ServiceBundle
{
    public delegate void NodeEditEventHandler(BaseNode node);

    public delegate void NodeUpdateCallbackHandler(BaseNode node);


    public readonly OSCService? OSC;
    public readonly PulsoidService? Pulsoid;

    public ServiceBundle(IServiceProvider services)
    {
        OSC = services.GetService<OSCService>();
        Pulsoid = services.GetService<PulsoidService>();
    }

    public event NodeEditEventHandler? OnNodeEdit;
    public event NodeUpdateCallbackHandler? OnNodeUpdate;

    public void EditNode(BaseNode node)
    {
        OnNodeEdit?.Invoke(node);
    }

    public void UpdateNode(BaseNode node)
    {
        OnNodeUpdate?.Invoke(node);
    }
}