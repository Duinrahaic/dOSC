using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public partial class OSCFloatBlock
    {
        [Parameter]
        public OSCFloatNode Node { get; set; } = null;
    }
}
