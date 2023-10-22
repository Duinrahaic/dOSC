using dOSC.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Connectors.OSC
{
    public partial class OSCIntBlock
    {
        [Parameter] 
        public OSCIntNode Node { get; set; } = null;
    }
}
