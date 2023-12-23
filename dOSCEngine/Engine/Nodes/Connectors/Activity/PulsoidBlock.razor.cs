using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Connectors.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.Activity
{
    public partial class PulsoidBlock
    {

        [Parameter] public PulsoidNode Node { get; set; } = null;
    }
}
