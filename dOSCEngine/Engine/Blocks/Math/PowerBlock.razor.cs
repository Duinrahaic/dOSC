using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class PowerBlock
    {
        [Parameter]
        public PowerNode Node { get; set; } = null;
    }
}
