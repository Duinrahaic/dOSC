using dOSCEngine.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Connectors.OSC
{
    public partial class OSCFloatReadBlock
    {
        [Parameter]
        public OSCFloatReadNode Node { get; set; } = null;
    }
}
