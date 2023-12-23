using dOSCEngine.Components.Modals;
using dOSCEngine.Engine.Nodes;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public partial class OSCIntBlock
    {
        [Parameter]
        public OSCIntNode Node { get; set; } = null;
    }
}
