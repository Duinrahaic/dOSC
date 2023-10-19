using dOSC.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;

namespace dOSC.Components.Wiresheet.Blocks.Logic
{
    public partial class NotEqualBlock
    {
        [Parameter] public NotEqualNode Node { get; set; } = null;

        
    }
}
