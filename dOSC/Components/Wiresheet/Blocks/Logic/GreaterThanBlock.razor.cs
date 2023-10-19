using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class GreaterThanBlock
    {
        [Parameter] public GreaterThanNode Node { get; set; } = null;

    }
}
