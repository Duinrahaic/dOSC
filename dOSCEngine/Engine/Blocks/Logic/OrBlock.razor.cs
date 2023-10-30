using dOSCEngine.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Logic
{
    public partial class OrBlock
    {
        [Parameter] public OrNode Node { get; set; } = null;
    }
}
