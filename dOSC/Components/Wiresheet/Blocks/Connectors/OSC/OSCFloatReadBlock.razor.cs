using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC
{
    public partial class OSCFloatReadBlock
    {
        [Parameter] 
        public OSCFloatReadNode Node { get; set; } = null;
    }
}
