using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class LessThenBlock
    {
        [Parameter] public LessThanNode Node { get; set; } = null;

    }
}
