using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class PowerBlock
    {
        [Parameter]
        public PowerNode Node { get; set; } = null;
    }
}
