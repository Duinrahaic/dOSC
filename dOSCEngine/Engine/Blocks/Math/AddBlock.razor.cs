using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class AddBlock
    {
        [Parameter] public AddNode Node { get; set; } = null;
    }
}
