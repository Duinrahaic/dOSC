using dOSCEngine.Engine.Nodes;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using dOSCEngine.Services.Connectors.OSC;
using Microsoft.Extensions.DependencyInjection;

namespace dOSCEngine.Services
{
    public class ServiceBundle
    {
        public delegate void NodeEditEventHandler(BaseNode node);
        public event NodeEditEventHandler? OnNodeEdit;
        public delegate void NodeUpdateCallbackHandler(BaseNode node);
        public event NodeUpdateCallbackHandler? OnNodeUpdate;

        
        
        public readonly OSCService? OSC;
        public readonly PulsoidService? Pulsoid;
 
        public ServiceBundle(IServiceProvider services)
        {
            OSC = services.GetService<OSCService>();
            Pulsoid = services.GetService<PulsoidService>();
         }

        public void EditNode(BaseNode node)
        {
            OnNodeEdit?.Invoke(node);
        }

        public void UpdateNode(BaseNode node)
        {
            OnNodeUpdate?.Invoke(node);
        }

        






    }
}
