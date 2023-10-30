using dOSCEngine.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;

namespace dOSCEngine.Engine.Blocks.Logic
{
    public partial class NotEqualBlock
    {
        [Parameter] public NotEqualNode Node { get; set; } = null;


    }
}
