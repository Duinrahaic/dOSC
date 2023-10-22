using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC
{
    public partial class OSCBooleanBlock
    {
        [Parameter] 
        public OSCBooleanNode Node { get; set; } = null;
    }
}
