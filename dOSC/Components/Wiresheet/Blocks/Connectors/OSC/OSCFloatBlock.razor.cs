using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC
{
    public partial class OSCFloatBlock
    {
        [Parameter] 
        public OSCFloatNode Node { get; set; } = null;
    }
}
