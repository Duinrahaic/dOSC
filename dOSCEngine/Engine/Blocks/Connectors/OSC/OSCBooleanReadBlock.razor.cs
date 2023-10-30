using dOSCEngine.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Connectors.OSC
{
    public partial class OSCBooleanReadBlock
    {
        [Parameter]
        public OSCBooleanReadNode Node { get; set; } = null;
    }
}
