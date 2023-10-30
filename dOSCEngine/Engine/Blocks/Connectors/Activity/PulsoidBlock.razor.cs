using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Connector.Activity;
using dOSCEngine.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Connectors.Activity
{
    public partial class PulsoidBlock
    {
        
        [Parameter] public PulsoidNode Node { get; set; } = null;
    }
}
