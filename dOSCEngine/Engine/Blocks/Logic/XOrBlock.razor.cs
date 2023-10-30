using dOSCEngine.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Logic
{
    public partial class XOrBlock
    {
        [Parameter] public XOrNode Node { get; set; } = null;

    }
}
