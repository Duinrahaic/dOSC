using dOSCEngine.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Logic
{
    public partial class NotBlock
    {
        [Parameter] public NotNode Node { get; set; } = null;

    }
}
