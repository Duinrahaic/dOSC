using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC
{
    public partial class OSCBooleanReadBlock
    {
        [Parameter] 
        public OSCBooleanReadNode Node { get; set; } = null;
    }
}
