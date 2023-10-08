using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class NotBlock
    {
        [Parameter] public NotNode Node { get; set; } = null;

    }
}
