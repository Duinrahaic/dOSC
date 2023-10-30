using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class ClampBlock
    {
        [Parameter] public ClampNode Node { get; set; } = null;
    }
}
