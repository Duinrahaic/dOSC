using dOSC.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Math
{
    public partial class AddBlock
    {
        [Parameter] public AddNode Node { get; set; } = null;
    }
}
