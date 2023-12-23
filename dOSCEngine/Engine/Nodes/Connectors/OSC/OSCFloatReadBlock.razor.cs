using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public partial class OSCFloatReadBlock
    {
        [Parameter]
        public OSCFloatReadNode Node { get; set; } = null;
    }
}
