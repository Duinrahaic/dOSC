using Microsoft.AspNetCore.Components;
using E = dOSCEngine.Engine.Nodes.Logic;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public partial class AndBlock
    {
        [Parameter] public AndNode Node { get; set; } = null;

    }
}
