using dOSC.Engine.Nodes.Connector.Activity;
using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.Activity
{
    public partial class PulsoidBlock
    {
        [Parameter] public PulsoidNode Node { get; set; } = null;
    }
}
