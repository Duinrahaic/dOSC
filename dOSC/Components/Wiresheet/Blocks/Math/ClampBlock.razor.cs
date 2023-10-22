using dOSC.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Math
{
    public partial class ClampBlock
    {
        [Parameter] public ClampNode Node { get; set; } = null; 
    }
}
