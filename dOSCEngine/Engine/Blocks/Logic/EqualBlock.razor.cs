using dOSCEngine.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;

namespace dOSCEngine.Engine.Blocks.Logic
{
    public partial class EqualBlock
    {
        [Parameter] public EqualNode Node { get; set; } = null;


    }
}
