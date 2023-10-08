using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class XOrBlock
    {
        [Parameter] public XOrNode Node { get; set; } = null;

    }
}
