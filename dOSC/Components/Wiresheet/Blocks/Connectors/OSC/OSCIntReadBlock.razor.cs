using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC
{
    public partial class OSCIntReadBlock
    {
        [Parameter] 
        public OSCIntReadNode Node { get; set; } = null;
    }
}
