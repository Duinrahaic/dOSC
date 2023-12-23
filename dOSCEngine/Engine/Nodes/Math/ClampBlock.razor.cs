using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class ClampBlock
    {
        [Parameter] public ClampNode Node { get; set; } = null;
    }
}
