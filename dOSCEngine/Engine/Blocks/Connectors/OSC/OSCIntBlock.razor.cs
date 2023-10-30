using dOSCEngine.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Connectors.OSC
{
    public partial class OSCIntBlock
    {
        [Parameter]
        public OSCIntNode Node { get; set; } = null;
    }
}
