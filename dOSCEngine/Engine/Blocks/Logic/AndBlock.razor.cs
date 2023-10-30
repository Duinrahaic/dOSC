using dOSCEngine.Engine.Nodes.Logic;
using Microsoft.AspNetCore.Components;
using E = dOSCEngine.Engine.Nodes.Logic;

namespace dOSCEngine.Engine.Blocks.Logic
{
    public partial class AndBlock
    {
        [Parameter] public AndNode Node { get; set; } = null;

    }
}
