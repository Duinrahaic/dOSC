using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class OrBlock
    {
        [Parameter] public OrNode Node { get; set; } = null;
    }
}
