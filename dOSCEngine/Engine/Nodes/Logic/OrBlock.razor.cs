using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public partial class OrBlock
    {
        [Parameter] public OrNode Node { get; set; } = null;
    }
}
